using Data.Entities.Persons.Roles;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.Role;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageRoles")]
public class ManageRolesController : Controller
{
    #region ConstructorInjection

    private readonly IPermissionServices _permissionServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageRolesController(IPermissionServices permissionServices, ITransactionDbContextServices transactions)
    {
        _permissionServices = permissionServices;
        _transactions = transactions;
    }

    #endregion

    [Route("/Admin/ManageRoles")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("GetRoles/GetAllRoles")]
    [Route("GetRoles/GetDeletedRoles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        IEnumerable<RoleDto> roles;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllRoles"))
            roles = await _permissionServices.GetRolesAsync(cancellationToken, isDeleted: false);
        else
            roles = await _permissionServices.GetRolesAsync(cancellationToken, isDeleted: true);

        return PartialView("GetRole", roles);
    }


    #region AddRole

    [Route("AddRole")]
    public IActionResult AddRole()
    {
        return View();
    }

    [Route("AddRole")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _permissionServices.IsRoleExistAsync(model.RoleName.Trim(), cancellationToken))
        {
            ModelState.AddModelError(nameof(model.RoleName), "نام نقش مورد نظر قبلا ثبت شده است.لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        #region AddRole

        var role = new Role
        {
            RoleName = model.RoleName,
            CreateDate = DateTime.Now,
            CanDeleteOrEdit = model.CanDeleteOrEdit,
        };

        await _permissionServices.AddRoleAsync(role, cancellationToken);

        #endregion

        // TODO AddPermissionsToRole

        return RedirectToAction("Index");
    }

    #endregion


    #region EditRole

    [Route("EditRole/{roleId:int}")]
    public async Task<IActionResult> EditRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId, cancellationToken, isDeleted: false, withTracking: false);
        if (role == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditRole"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditRole"];
            TempData.Remove("ConcurrencyInEditRole");
        }

        var model = EditRoleDto.FromEntity(role);

        return View(model);
    }

    [Route("EditRole")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleDto model, string preRoleName, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.RoleName.Trim() != preRoleName && await _permissionServices.IsRoleExistAsync(model.RoleName.Trim(), cancellationToken))
        {
            ModelState.AddModelError(nameof(model.RoleName), "نام نقش مورد نظر قبلا ثبت شده است.لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        var role = await _permissionServices.GetRoleAsync(model.RoleId, cancellationToken, isDeleted: false);
        if (role == null) return NotFound();

        // ConCurrency Check
        if (Convert.ToBase64String(role.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditRole"] = true;
            return Redirect($"/Admin/EditRole/{model.RoleId}");
        }

        #region UpdateRole

        model.ToEntity(role);
        await _transactions.SaveChangesAsync(cancellationToken);

        #endregion

        return RedirectToAction("Index");
    }

    #endregion 


    #region Delete And Restore Role

    [Route("DeleteRole/{roleId:int}")]
    [HttpPost]
    public async Task<IActionResult> DeleteRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId, cancellationToken, isDeleted: false); //نقش مورد نظر نباید در حذف شده ها باشد.اگر در حذف شده ها نباشد پر است و اگر در حذف شده ها باشد چیزی بر نمیگرداند.
        if (role == null) return PartialView("_Error404");

        role.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageRoles/GetRoles/GetAllRoles");
    }

    [Route("RestoreRole/{roleId:int}")]
    [HttpPost]
    public async Task<IActionResult> RestoreRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId, cancellationToken, isDeleted: true); //نقش مورد نظر باید در حذف شده ها باشد تا بازگردانی شود.اگر حذف شده نباشد چیزی بر نمیگرداند
        if (role == null) return PartialView("_Error404");

        role.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageRoles/GetRoles/GetDeletedRoles");
    }

    #endregion
}
