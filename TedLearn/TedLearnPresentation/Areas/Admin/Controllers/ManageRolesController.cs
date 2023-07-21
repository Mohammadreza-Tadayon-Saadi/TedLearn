using Data.Entities.Persons.Roles;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts.Interfaces;
using Services.DTOs.AdminPanel.Role;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ManageRolesController : Controller
{
    #region ConstructorInjection

    private readonly IPermissionServices _permissionServices;
    public ManageRolesController(IPermissionServices permissionServices)
    {
        _permissionServices = permissionServices;
    }

    #endregion

    [Route("/Admin/ManageRoles")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/GetRoles/{message}")]
    public async Task<IActionResult> GetRoles(string message , CancellationToken cancellationToken)
    {
        if (message == "GetAllRoles")
        {
            var allRoles = await _permissionServices.GetRolesAsync(cancellationToken , isDeleted:false);
            return PartialView("GetRole", allRoles);
        }
        if (message == "GetDeletedRoles")
        {
            var deletedRoles = await _permissionServices.GetRolesAsync(cancellationToken, isDeleted: true);
            return PartialView("GetRole", deletedRoles);
        }
        return NotFound();
    }


    #region AddRole

    [Route("/Admin/ManageRoles/AddRole")]
    [Route("/Admin/AddRole")]
    public IActionResult AddRole()
    {
        return View();
    }

    [Route("/Admin/ManageRoles/AddRole")]
    [Route("/Admin/AddRole")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleDto model , CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _permissionServices.IsRoleExistAsync(model.RoleName.Trim() , cancellationToken))
        {
            ModelState.AddModelError(nameof(model.RoleName), "نام نقش مورد نظر قبلا ثبت شده است.لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        #region AddRole

        var role = new Role
        {
            RoleName = model.RoleName.Trim(),
            CreateDate = DateTime.Now,
            CanDeleteOrEdit = model.CanDeleteOrEdit,
        };

        await _permissionServices.AddRoleAsync(role , cancellationToken);

        #endregion

        // TODO AddPermissionsToRole
        
        return RedirectToAction("Index");
    }

    #endregion


    #region EditRole

    [Route("/Admin/ManageRoles/EditRole/{roleId:int}")]
    [Route("/Admin/EditRole/{roleId:int}")]
    public async Task<IActionResult> EditRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId, cancellationToken, isDeleted: false , withTracking: false);
        if (role == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditRole"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditRole"];
            TempData.Remove("ConcurrencyInEditRole");
        }

        var model = EditRoleDto.FromEntity(role);

        return View(model);
    }

    [Route("/Admin/ManageRoles/EditRole")]
    [Route("/Admin/EditRole")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleDto model, string preRoleName, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.RoleName.Trim() != preRoleName && await _permissionServices.IsRoleExistAsync(model.RoleName.Trim() ,cancellationToken))
        {
            ModelState.AddModelError(nameof(model.RoleName), "نام نقش مورد نظر قبلا ثبت شده است.لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        var role = await _permissionServices.GetRoleAsync(model.RoleId , cancellationToken , isDeleted:false);
        if (role == null) return NotFound();

        // ConCurrency Check
        if (Convert.ToBase64String(role.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditRole"] = true;
            return Redirect($"/Admin/EditRole/{model.RoleId}");
        }

        #region UpdateRole

        model.ToEntity(role);
        //role.RoleName = model.RoleName.Trim();
        //role.CanDeleteOrEdit = model.CanDeleteOrEdit;
        await _permissionServices.UpdateRoleAsync(role, cancellationToken);

        #endregion

        return RedirectToAction("Index");
    }

    #endregion 


    //حذف و یا بازگردانی نقش مورد نظر
    #region Delete And Restore Role

    [Route("/Admin/ManageRoles/DeleteRole/{roleId:int}")]
    [Route("/Admin/DeleteRole/{roleId:int}")]
    [HttpPost]
    public async Task<IActionResult> DeleteRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId  , cancellationToken , isDeleted:false); //نقش مورد نظر نباید در حذف شده ها باشد.اگر در حذف شده ها نباشد پر است و اگر در حذف شده ها باشد چیزی بر نمیگرداند.
        if (role == null) return NotFound();

        role.IsDelete = true;
        await _permissionServices.UpdateRoleAsync(role , cancellationToken);

        return Redirect("/GetRoles/GetAllRoles");
    }

    [Route("/Admin/ManageRoles/RestoreRole/{roleId:int}")]
    [Route("/Admin/RestoreRole/{roleId:int}")]
    [HttpPost]
    public async Task<IActionResult> RestoreRole(int roleId, CancellationToken cancellationToken)
    {
        var role = await _permissionServices.GetRoleAsync(roleId , cancellationToken, isDeleted: true); //نقش مورد نظر باید در حذف شده ها باشد تا بازگردانی شود.اگر حذف شده نباشد چیزی بر نمیگرداند
        if (role == null) return NotFound();

        role.IsDelete = false;
        await _permissionServices.UpdateRoleAsync(role, cancellationToken);

        return Redirect("/GetRoles/GetDeletedRoles");
    }

    #endregion


    //نقش هایی که حذف شده اند
    #region DeletedRole

    [Route("/Admin/ManageRoles/DeletedRole")]
    [Route("/Admin/DeletedRole")]
    public IActionResult DeletedRole()
    {
        return View();
    }

    #endregion
}
