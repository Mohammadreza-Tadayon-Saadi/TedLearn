using Core.Convertors;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.UserDiscount;
using System.Threading;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageUserDiscounts")]
public class ManageUserDiscountsController : Controller
{
    #region ConstructorInjection

    private readonly IDiscountServices _discountServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageUserDiscountsController(IDiscountServices discountServices, ITransactionDbContextServices transactions)
    {
        _discountServices = discountServices;
        _transactions = transactions;
    }

    #endregion  

    [Route("")]
    public IActionResult Index()
    {
        if (TempData.ContainsKey("AddDiscount"))
        {
            ViewBag.AddDiscount = TempData["AddDiscount"];
            TempData.Remove("AddDiscount");
        }
        if (TempData.ContainsKey("EditDiscount"))
        {
            ViewBag.EditDiscount = TempData["EditDiscount"];
            TempData.Remove("EditDiscount");
        }

        return View();
    }

    #region Ajax/View

    [Route("GetUserDiscounts/GetActiveUserDiscounts")]
    [Route("GetUserDiscounts/GetDeletedUserDiscounts")]
    public async Task<IActionResult> GetUserDiscounts(CancellationToken cancellationToken , int pageId = 1 , int take = 20)
    {
        FilteredUserDiscountDto filteredUserDiscount;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetActiveUserDiscounts"))
            filteredUserDiscount = await _discountServices.GetUserDiscountListAsync(ud => ud.DiscountId , pageId  , take, cancellationToken, isDeleted: false);
        else
            filteredUserDiscount = await _discountServices.GetUserDiscountListAsync(ud => ud.DiscountId, pageId, take, cancellationToken, isDeleted: true);

        return PartialView("GetUserDiscountAjax", filteredUserDiscount);
    }

    #endregion


    #region AddDiscounts

    [Route("AddDiscount")]
    public IActionResult AddDiscount() => View();

    [Route("AddDiscount")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddDiscount(UserDiscountDto model ,CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid) return View(model);

        var startDate = DateConvertors.ShamsiToMiladi(model.StartDateShamsi);
        if (startDate < DateTime.Now)
            ModelState.AddModelError(nameof(model.StartDateShamsi), "تاریخ شروع تخفیف فقط میتواند از امروز به بعد باشد.");

        var endDate = DateConvertors.ShamsiToMiladi(model.EndDateShamsi);
        if (endDate < DateTime.Now)
            ModelState.AddModelError(nameof(model.EndDateShamsi), "تاریخ اتمام تخفیف فقط میتواند از امروز به بعد باشد.");

        if (startDate > endDate)
            ModelState.AddModelError(nameof(model.EndDateShamsi), "تاریخ اتمام تخفیف باید بعد از شروع تخفیف باشد.");

        if (await _discountServices.IsExistsUDiscountCodeAsync(model.DiscountCode.Trim()))
            ModelState.AddModelError(nameof(model.DiscountCode), "کد تخفیف وارد شده در سرور موجود میباشد.لطفا کد تخفیف دیگری انتخاب کنید.");

        if (!ModelState.IsValid) return View(model);

        #endregion

        var uDiscount = model.ToEntity();
        uDiscount.StartDate = startDate;
        uDiscount.EndDate = endDate;

        await _discountServices.AddUDiscountAsync(uDiscount , cancellationToken);

        TempData["AddDiscount"] = true;

        return Redirect("/Admin/ManageUserDiscounts");
    }

    #endregion


    #region EditDiscount

    [Route("EditDiscount/{discountId:int}")]
    public async Task<IActionResult> EditDiscount(int discountId , CancellationToken cancellationToken)
    {
        var discount = await _discountServices.GetUDiscountByIdAsync(discountId , cancellationToken , withTracking: false , getActive: true);
        if (discount == null) return NotFound();

        var model = UserDiscountDto.FromEntity(discount);

        if (TempData.ContainsKey("ConcurrencyInEditUDiscount"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditUDiscount"];
            TempData.Remove("ConcurrencyInEditUDiscount");
        }

        return View(model);
    }

    [Route("EditDiscount")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditDiscount(UserDiscountDto model, string preDiscountCode , CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid) return View(model);

        var startDate = DateConvertors.ShamsiToMiladi(model.StartDateShamsi);
        if (startDate < DateTime.Now)
            ModelState.AddModelError(nameof(model.StartDateShamsi), "تاریخ شروع تخفیف فقط میتواند از امروز به بعد باشد.");

        var endDate = DateConvertors.ShamsiToMiladi(model.EndDateShamsi);
        if (endDate < DateTime.Now)
            ModelState.AddModelError(nameof(model.EndDateShamsi), "تاریخ اتمام تخفیف فقط میتواند از امروز به بعد باشد.");

        if (startDate > endDate)
            ModelState.AddModelError(nameof(model.EndDateShamsi), "تاریخ اتمام تخفیف باید بعد از شروع تخفیف باشد.");

        if (model.DiscountCode != preDiscountCode && await _discountServices.IsExistsUDiscountCodeAsync(model.DiscountCode.Trim()))
            ModelState.AddModelError(nameof(model.DiscountCode), "کد تخفیف وارد شده در سرور موجود میباشد.لطفا کد تخفیف دیگری انتخاب کنید.");

        if (!ModelState.IsValid) return View(model);

        #endregion

        var discount = await _discountServices.GetUDiscountByIdAsync((int)model.DiscountId, cancellationToken, withTracking: true, getActive: true);
        if (discount == null) return NotFound();

        if (model.Base64Version != discount.Version.ToBase64String())
        {
            TempData["ConcurrencyInEditUDiscount"] = true;
            return Redirect($"/Admin/ManageUserDiscounts/EditDiscount/{model.DiscountId}");
        }

        model.ToEntity(discount);
        discount.StartDate = startDate;
        discount.EndDate = endDate;

        await _transactions.SaveChangesAsync(cancellationToken);

        TempData["EditDiscount"] = true;

        return Redirect("/Admin/ManageUserDiscounts");
    }

    #endregion


    #region DeleteDiscount

    [Route("DeleteDiscount/{discountId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteDiscount(int discountId , CancellationToken cancellationToken)
    {
        var discount = await _discountServices.GetUDiscountByIdAsync(discountId , cancellationToken ,  getActive: true); // کاربر نباید حذف شده باشد
        if (discount == null) return PartialView("_Error404");

        discount.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageUserDiscounts/GetUserDiscounts/GetActiveUserDiscounts");
    }

    #endregion


    #region RestoreDiscount

    [Route("RestoreDiscount/{discountId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreDiscount(int discountId , CancellationToken cancellationToken)
    {
        var discount = await _discountServices.GetUDiscountByIdAsync(discountId, cancellationToken, getActive: false); // کاربر نباید حذف شده باشد
        if (discount == null) return PartialView("_Error404");

        discount.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageUserDiscounts/GetUserDiscounts/GetDeletedUserDiscounts");
    }

    #endregion
}
