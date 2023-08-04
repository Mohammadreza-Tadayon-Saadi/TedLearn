using Core.Securities;
using Core.Utilities;
using Data.Entities.Persons.Users;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.Role;
using Services.DTOs.AdminPanel.User;

namespace TedLearn_Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
[Route("/Admin/ManageUsers")]
public class ManageUsersController : Controller
{
    #region ConstructorInjection

    private readonly IUserServices _userServices;
    private readonly IUserPanelServices _userPanelServices;
    private readonly IPermissionServices _permissionServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageUsersController(IUserServices userServices, IPermissionServices permissionServices,
        IUserPanelServices userPanelServices, ITransactionDbContextServices transactions)
    {
        _userServices = userServices;
        _permissionServices = permissionServices;
        _userPanelServices = userPanelServices;
        _transactions = transactions;
    }

    #endregion


    [Route("/Admin/ManageUsers")]
    public IActionResult Index()
    {
        return View();
    }


    #region Ajax/View

    [Route("GetUsers/GetAllUsers")]
    [Route("GetUsers/GetDeletedUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken, string? orderBy, int pageId = 1)
    {
        GetUsersDto users;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllUsers"))
            users = await _userServices.GetAllUsersAsync(deletedUser: false, cancellationToken, orderBy, pageId);
        else
            users = await _userServices.GetAllUsersAsync(deletedUser: true, cancellationToken, orderBy, pageId);
        
        return PartialView("GetUser", users);
    }

    [Route("GetUserInformation/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> GetUserInformation(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserInformationAsync(userId, cancellationToken);

        if (user == null) return PartialView("_Error404");

        return PartialView("GetUserInformation", user);
    }

    #endregion


    #region AddUser

    [Route("AddUser")]
    public IActionResult AddUser()
    {
        return View();
    }

    [Route("AddUser")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        // PhoneNumber Or UserName Exists In DataBase
        if (await _userServices.IsUserNameOrPhoneNumberExistAsync(model.UserName.Trim(), model.PhoneNumber, cancellationToken))
        {
            ViewBag.UserExist = true;
            return View(model);
        }

        var newUser = new User()
        {
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber,
            PasswordHash = SecurityHelper.GetSha256Hash(model.Password.Trim()),
            UserAvatar = "Default.png",
            RegisterDate = DateTime.Now,
            PhoneNumberConfirmed = model.PhoneNumberConfirmed
        };

        await _userServices.SignUpUserAsync(newUser, cancellationToken);

        return RedirectToAction("Index");
    }

    #endregion


    #region EditUser

    [Route("EditUser/{userId:int}")]
    public async Task<IActionResult> EditUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserForEditInAdminAsync(userId, cancellationToken);
        if (user == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditUser"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditUser"];
            TempData.Remove("ConcurrencyInEditUser");
        }

        return View(user);
    }

    [Route("EditUser")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserDto model, string prePhoneNumber, CancellationToken cancellationToken)
    {
        #region ValidateInput

        if (!ModelState.IsValid) return View(model);

        if (model.PhoneNumber != prePhoneNumber && await _userServices.IsPhoneExistAsync(model.PhoneNumber, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.PhoneNumber), "شماره تلفن وارد شده صحیح نیست.");
            return View(model);
        }

        #endregion

        var user = await _userServices.GetUserByIdAsync(model.UserId, cancellationToken);

        // Concurrency Check
        if (Convert.ToBase64String(user.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditUser"] = true;
            return Redirect($"/ManageUsers/EditUser/{model.UserId}");
        }

        #region UpdateUser

        var modelPassword = model.Password;
        if (model.Password.HasValue())
            model.Password = SecurityHelper.GetSha256Hash(model.Password.Trim());

        model.ToEntity(user);

        // Both Methods Must Runs Without Error.If An Error Occures,The Transaction Will Be RollBacked. 
        var result = await _transactions.ExecuteInTransactionAsync(async () =>
        {
            // We Do Not Need To Update Whole User's Column
            //await _userServices.UpdateUserAsync(user, cancellationToken , withSaveChanges:false);

            //Charge Wallet IF Amount Exists
            if (model.Amount.HasValue)
                await _userPanelServices.ChargeWalletAsync(model.UserId, (decimal)model.Amount, "شارژ از طرف ادمین", cancellationToken
                    , isPay: true, withSaveChanges: false);

        }, cancellationToken, false);

        if (result == 0)
        {
            ViewBag.ProccessFailed = true;
            model.Password = modelPassword; // maybe password be hashed!
            return View(model);
        }

        #endregion

        return Redirect("/Admin/ManageUsers");
    }

    #endregion


    #region DeleteUser

    [Route("DeleteUser/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserByIdAsync(userId, cancellationToken, getActive: true); // کاربر نباید حذف شده باشد
        if (user == null) return PartialView("_Error404");

        user.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageUsers/GetUsers/GetAllUsers");
    }

    #endregion


    #region RestoreUser

    [Route("RestoreUser/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserByIdAsync(userId, cancellationToken, getActive: false); // کاربر نباید حذف شده باشد
        if (user == null) return PartialView("_Error404");
        user.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageUsers/GetUsers/GetDeletedUsers");
    }

    #endregion


    #region UserRoles

    [Route("UserRoles/{userId:int}")]
    public async Task<IActionResult> GetRolesForUser(int userId, CancellationToken cancellationToken)
    {
        if (!await _userServices.IsUserExistAsync(userId, cancellationToken)) return NotFound();

        ViewBag.UserId = userId;

        return View();
    }

    [Route("GetRolesForUserAjax/{userId:int}")]
    public async Task<IActionResult> GetRolesForUserAjax(int userId, CancellationToken cancellationToken)
    {
        if (!await _userServices.IsUserExistAsync(userId, cancellationToken)) return PartialView("_Error404");

        var userRoles = await _permissionServices.GetRolesForUserAsync(userId, cancellationToken);

        return PartialView("GetRolesForUserAjax", userRoles);
    }

    #endregion


    #region AddRoleToUser

    [Route("AddRoleToUser/{userId:int}/{roleId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(int userId, int roleId, CancellationToken cancellationToken)
    {
        var userRole = await _permissionServices.GetUserRoleAsync(userId, roleId, cancellationToken, withTracking: false);
        if (userRole != null) return PartialView("_Error404");

        userRole = new Data.Entities.Persons.Roles.UserRole
        {
            UserId = userId,
            RoleId = roleId,
        };

        await _permissionServices.AddRoleToUserAsync(userRole, cancellationToken);

        return Redirect($"/Admin/ManageUsers/GetRolesForUserAjax/{userId}");
    }

    #endregion


    #region DeleteRoleFromUser

    [Route("DeleteRoleFromUser/{userId:int}/{roleId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteRoleFromUser(int userId, int roleId, CancellationToken cancellationToken)
    {
        var userRole = await _permissionServices.GetUserRoleAsync(userId, roleId, cancellationToken);
        if (userRole == null) return PartialView("_Error404");

        await _permissionServices.DeleteRoleFromUserAsync(userRole, cancellationToken);

        return Redirect($"/Admin/ManageUsers/GetRolesForUserAjax/{userId}");
    }

    #endregion
}
