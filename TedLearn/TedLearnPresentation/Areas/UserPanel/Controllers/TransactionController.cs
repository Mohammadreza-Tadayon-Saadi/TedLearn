using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts.Interfaces;
using Services.DTOs.UserPanel.Transaction;

namespace TedLearnPresentation.Areas.UserPanel.Controllers;

[Area("UserPanel")]
[Authorize]
public class TransactionController : Controller
{
    #region ConstructorInjection

    private readonly IUserPanelServices _userPanelServices;
    private readonly IUserServices _userServices;
    public TransactionController(IUserPanelServices userPanelServices, IUserServices userServices)
    {
        _userPanelServices = userPanelServices;
        _userServices = userServices;
    }

    #endregion


    #region AddTransaction

    [Route("/UserPanel/AddTransaction")]
    public async Task<IActionResult> AddTransaction(CancellationToken cancellationToken ,bool buyCourses = false)
    {
        var userStock = await _userPanelServices.GetStockForUserAsync(User.Identity.GetUserId<int>() , cancellationToken);
        ViewBag.BuyCourses = buyCourses;

        var model = new TransactionDto { Stock = userStock };
        return View(model);
    }

    [Route("/UserPanel/AddTransaction")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddTransaction(TransactionDto model, CancellationToken cancellationToken, bool buyCourses = false)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.Identity.GetUserId<int>();

        var transactionId = await _userPanelServices.ChargeWalletAsync(userId, model.Amount, "شارژ حساب" , cancellationToken);

        var peyment = new ZarinpalSandbox.Payment(((int)model.Amount));
        var result = await peyment.PaymentRequest("شارژ کیف پول", $"https://localhost:7199/OnlinePayment/{transactionId}?buyCourses={buyCourses}", "mohammadrezatedi83@gmail.com", "09032594838");

        if (result.Status == 100)
            return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");

        ViewBag.IsNotSuccess = true;

        return View(model);
    }

    #endregion


    #region GetTransactionForUser

    [Route("/Home/UserPanel/GetUserTransactions")]
    [Route("/UserPanel/GetUserTransactions")]
    public async Task<IActionResult> GetUserTransactions(CancellationToken cancellationToken , int pageId = 1)
    {
        var model = await _userPanelServices.GetTransationsForUser(User.Identity.GetUserId<int>() , cancellationToken, pageId);

        return PartialView("GetUserTransactions", model);
    }

    #endregion


    #region OnlinePayment

    [Route("/OnlinePayment/{transactionId:int}")]
    public async Task<IActionResult> OnlinePayment(int transactionId , CancellationToken cancellationToken , bool buyCourses = false)
    {
        var transaction = await _userPanelServices.GetTransactionByTransactionIdAsync(transactionId , cancellationToken);
        if (transaction == null)
        {
            ViewBag.IsInValidTransaction = true;
            return View();
        }

        if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
        {
            string authority = HttpContext.Request.Query["Authority"];

            var payment = new ZarinpalSandbox.Payment((int)transaction.Amount);
            var result = payment.Verification(authority).Result;

            if (result.Status == 100)
            {
                transaction.IsPay = true;

                await _userPanelServices.UpdateTransactionAsync(transaction , cancellationToken);

                ViewBag.TrackingCode = result.RefId;
                ViewBag.IsSuccess = true;

                if (buyCourses)
                {
                    TempData["BuyCourse"] = true;
                    return Redirect("/UserPanel/MyOrders");
                }
            }
            else
            {
                ViewBag.IsInValidTransaction = true;
                await _userPanelServices.RemoveTransactionAsync(transaction, cancellationToken);
            }
        }
        else
        {
            ViewBag.IsInValidTransaction = true;
            await _userPanelServices.RemoveTransactionAsync(transaction, cancellationToken);
        }

        return View();
    }

    #endregion
}
