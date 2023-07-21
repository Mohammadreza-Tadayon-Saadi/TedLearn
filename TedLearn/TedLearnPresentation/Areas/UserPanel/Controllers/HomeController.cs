using Core.Securities;
using Core.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts.Interfaces;
using Services.DTOs.UserPanel;

namespace TedLearnPresentation.Areas.UserPanel.Controllers;

[Area("UserPanel")]
[Authorize]
public class HomeController : Controller
{
    #region ConstructorInjection

    private readonly IUserServices _userServices;
    private readonly IUserPanelServices _userPanelServices;
    public HomeController(IUserServices userServices, IUserPanelServices userPanelServices)
    {
        _userServices = userServices;
        _userPanelServices = userPanelServices;
    }


    #endregion


    [Route("/Home/UserPanel")]
    [Route("/UserPanel")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userInformation = await _userPanelServices.GetUserInformation(User.Identity.GetUserId<int>() , cancellationToken);

        if (TempData.ContainsKey("ResultOfUpdateAvatar"))
        {
            ViewBag.StateOfUpdateAvatar = TempData["ResultOfUpdateAvatar"];
            TempData.Remove("ResultOfUpdateAvatar");
        }

        return View(userInformation);
    }

    #region EditUserProfile

    [Route("/Home/UserPanel/EditUserProfile")]
    [Route("/UserPanel/EditUserProfile")]
    public async Task<IActionResult> EditUserProfile(CancellationToken cancellationToken)
    {
        var model = await _userPanelServices.GetUserForEdit(User.Identity.GetUserId<int>(), cancellationToken);

        if (TempData.ContainsKey("EditUserConcurrencyExp"))
        {
            ViewBag.ConCurrency = TempData["EditUserConcurrencyExp"];
            TempData.Remove("EditUserConcurrencyExp");
        }

        return View(model);
    }

    [Route("/Home/UserPanel/EditUserProfile")]
    [Route("/UserPanel/EditUserProfile")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserProfileDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userServices.GetUserByIdAsync(User.Identity.GetUserId<int>(), cancellationToken);
        if (user == null) return NotFound();

        // Check Concurrency
        var currentStamp = Convert.ToBase64String(user.Version);
        if (currentStamp != model.Base64Version)
        {
            TempData["EditUserConcurrencyExp"] = true;
            return Redirect("/UserPanel/EditUserProfile");
        }

        model.ToEntity(user);

        await _userServices.UpdateUserAsync(user , cancellationToken);

        return Redirect("/UserPanel");
    }

    #endregion

    #region ChangePassword

    [Route("/Home/UserPanel/ChangePassword")]
    [Route("/UserPanel/ChangePassword")]
    public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken)
    {
        var versionOfUser = Convert.ToBase64String(await _userServices.GetVersionOfUserAsync(User.Identity.GetUserId<int>(), cancellationToken));

        if (TempData.ContainsKey("ChangePasswordConcurrencyExp"))
        {
            ViewBag.ConCurrency = TempData["ChangePasswordConcurrencyExp"];
            TempData.Remove("ChangePasswordConcurrencyExp");
        }

        var model = new ChangePasswordDto()
        {
            Version = versionOfUser
        };

        return View(model);
    }

    [Route("/Home/UserPanel/ChangePassword")]
    [Route("/UserPanel/ChangePassword")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userServices.CheckUserForLoginAsync(User.Identity.GetUserId<int>(), model.Password.Trim(), cancellationToken);
        if (user == null)
        {
            ModelState.AddModelError(nameof(model.Password), "رمز عبور وارد شده نادرست است.");
            return View(model);
        }

        // Concurrency Check
        if (Convert.ToBase64String(user.Version) != model.Version)
        {
            TempData["ChangePasswordConcurrencyExp"] = true;
            return Redirect("/UserPanel/ChangePassword");
        }

        // Dublicate Password
        var newPass = SecurityHelper.GetSha256Hash(model.NewPassword);
        if (user.PasswordHash == newPass)
        {
            ModelState.AddModelError(nameof(model.NewPassword), "رمز عبور وارد شده قبلا استفاده شده است.");
            return View(model);
        }

        user.PasswordHash = newPass;
        await _userServices.UpdateUserAsync(user , cancellationToken);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/Account/Login?ChangePassword=true");
    }

    #endregion

    #region ChangeAvatar

    [Route("/UserPanel/ChangeAvatar")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ChangeAvatar(IFormFile avatarFile, string avatarName, CancellationToken cancellationToken)
    {
        if (!avatarFile.IsImage() || avatarFile == null)
        {
            TempData["ResultOfUpdateAvatar"] = "NullFile";
            return Redirect("/UserPanel");
        }

        if (avatarFile.Length <= 5300000) // 5MB
        {
            var user = await _userServices.GetUserByIdAsync(User.Identity.GetUserId<int>(), cancellationToken);

            var avatar = "";
            var directoryOfAvatar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar");

            //Save New Image
            avatar = await FileHelper.CreateFileAsync(avatarFile, directoryOfAvatar);

            user.UserAvatar = avatar;
            await _userServices.UpdateUserAsync(user, cancellationToken);

            //Delete Old Image
            if (avatarName != "Default.png")
                FileHelper.DeleteFile(avatarName, directoryOfAvatar);

            TempData["ResultOfUpdateAvatar"] = "SuccessEdit";
            return Redirect("/UserPanel");
        }

        TempData["ResultOfUpdateAvatar"] = "ToMuchLength";
        return Redirect("/UserPanel");
    }

    #endregion
}
