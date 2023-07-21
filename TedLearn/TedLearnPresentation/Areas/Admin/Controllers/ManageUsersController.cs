using Core.Securities;
using Core.Utilities;
using Data.Entities.Persons.Users;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts.Interfaces;
using Services.DTOs.AdminPanel.User;

namespace TedLearn_Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class ManageUsersController : Controller
{
    #region ConstructorInjection

    private readonly IUserServices _userServices;
    private readonly IUserPanelServices _userPanelServices;
    private readonly IPermissionServices _permissionServices;
    public ManageUsersController(IUserServices userServices, IPermissionServices permissionServices, IUserPanelServices userPanelServices)
    {
        _userServices = userServices;
        _permissionServices = permissionServices;
        _userPanelServices = userPanelServices;
    }

    #endregion


    [Route("/Admin/ManageUsers")]
    [Route("/ManageUsers")]
    public IActionResult Index()
    {
        return View();
    }


    #region Ajax/View

    [Route("/GetUsers/{message}")]
    [HttpGet]
    public async Task<IActionResult> GetUsers(string message, CancellationToken cancellationToken, string? orderBy, int pageId = 1)
    {
        if (message == "GetAllUsers")
        {
            var allUsers = await _userServices.GetAllUsersAsync(cancellationToken, false, orderBy, pageId);
            return PartialView("GetUser", allUsers);
        }
        if (message == "GetDeletedUsers")
        {
            var deletedUsers = await _userServices.GetAllUsersAsync(cancellationToken, true, orderBy, pageId);
            return PartialView("GetUser", deletedUsers);
        }
        return NotFound();
    }

    [Route("/GetUserInformation/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> GetUserInformation(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserInformationAsync(userId, cancellationToken);

        if (user == null) return NotFound();

        return PartialView("GetUserInformation", user);
    }

    #endregion


    #region AddUser

    [Route("/Admin/ManageUsers/AddUser")]
    [Route("/ManageUsers/AddUser")]
    public IActionResult AddUser()
    {
        return View();
    }

    [Route("/Admin/ManageUsers/AddUser")]
    [Route("/ManageUsers/AddUser")]
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
            UserName = model.UserName.Trim(),
            PhoneNumber = model.PhoneNumber.Trim(),
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

    [Route("/Admin/ManageUsers/EditUser/{userId:int}")]
    [Route("/ManageUsers/EditUser/{userId:int}")]
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

    [Route("/Admin/ManageUsers/EditUser")]
    [Route("/ManageUsers/EditUser")]
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
        var result = await _userServices.ExecuteInTransactionAsync(async transaction =>
        {
            await _userServices.UpdateUserAsync(user, cancellationToken);

            //Charge Wallet IF Amount Exists
            if (model.Amount.HasValue)
                await _userPanelServices.ChargeWalletAsync(model.UserId, (decimal)model.Amount, "شارژ از طرف ادمین", cancellationToken);

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


    //حذف کاربر مورد نظر//
    #region DeleteUser

    [Route("/Admin/ManageUsers/DeleteUser/{userId:int}")]
    [Route("/ManageUsers/DeleteUser/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserByIdAsync(userId, cancellationToken, getActive: true); // کاربر نباید حذف شده باشد
        if (user == null) return NotFound();

        user.IsDelete = true;
        await _userServices.UpdateUserAsync(user, cancellationToken);

        return Redirect("/GetUsers/GetAllUsers");
    }

    #endregion


    //بازگردانی کاربری که حذف شده
    #region RestoreUser

    [Route("/Admin/ManageUsers/RestoreUser/{userId:int}")]
    [Route("/ManageUsers/RestoreUser/{userId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userServices.GetUserByIdAsync(userId, cancellationToken, getActive: false); // کاربر نباید حذف شده باشد
        if (user == null) return NotFound();

        user.IsDelete = false;
        await _userServices.UpdateUserAsync(user, cancellationToken);

        return Redirect("/GetUsers/GetDeletedUsers");
    }

    #endregion


    //کاربرانی که حذف شده اند
    #region DeletedUsers

    [Route("/Admin/ManageUsers/DeletedUsers")]
    [Route("/ManageUsers/DeletedUsers")]
    public IActionResult DeletedUsers()
    {
        return View();
    }

    #endregion


    #region UserRoles

    [Route("/Admin/ManageUsers/UserRoles/{userId:int}")]
    [Route("/ManageUsers/UserRoles/{userId:int}")]
    public async Task<IActionResult> GetRolesForUser(int userId, CancellationToken cancellationToken)
    {
        if (!await _userServices.IsUserExistAsync(userId, cancellationToken)) return NotFound();

        return View();
    }

    [Route("/GetRolesForUserAjax/{userId:int}")]
    public async Task<IActionResult> GetRolesForUserAjax(int userId, CancellationToken cancellationToken)
    {
        if (!await _userServices.IsUserExistAsync(userId, cancellationToken)) return NotFound();

        var userRoles = await _permissionServices.GetRolesForUserAsync(userId, cancellationToken);

        return PartialView("GetRolesForUserAjax", userRoles);
    }

    #endregion


    #region AddRoleToUser

    [Route("/Admin/ManageUsers/AddRoleToUser/{userId:int}/{roleId:int}")]
    [Route("ManageUsers/AddRoleToUser/{userId:int}/{roleId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(int userId, int roleId, CancellationToken cancellationToken)
    {
        var userRole = await _permissionServices.GetUserRoleAsync(userId, roleId, cancellationToken, withTracking:false);
        if (userRole != null) return NotFound();

        userRole = new Data.Entities.Persons.Roles.UserRole
        {
            UserId = userId,
            RoleId = roleId,
        };

        await _permissionServices.AddRoleToUserAsync(userRole, cancellationToken);

        return Redirect($"/GetRolesForUserAjax/{userId}");
    }

    #endregion


    #region DeleteRoleFromUser

    [Route("/Admin/ManageUsers/DeleteRoleFromUser/{userId:int}/{roleId:int}")]
    [Route("ManageUsers/DeleteRoleFromUser/{userId:int}/{roleId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteRoleFromUser(int userId, int roleId, CancellationToken cancellationToken)
    {
        var userRole = await _permissionServices.GetUserRoleAsync(userId, roleId, cancellationToken);
        if (userRole == null) return NotFound();

        await _permissionServices.DeleteRoleFromUserAsync(userRole, cancellationToken);

        return Redirect($"/GetRolesForUserAjax/{userId}");
    }

    #endregion
}
