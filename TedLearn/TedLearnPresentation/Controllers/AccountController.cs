using Core.Generators;
using Core.Securities;
using Data.Entities.Persons.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Services.DTOs.Account;

namespace TedLearnPresentation.Controllers;

public class AccountController : Controller
{
    #region ConstructorInjection

    private readonly IUserServices _userServices;
    private readonly IPermissionServices _permissionServices;
    private readonly ITransactionDbContextServices _transactions;
    private readonly double expirationActivateCodeTime;
    public AccountController(IUserServices userServices, IConfiguration configuration, 
        IPermissionServices permissionServices, ITransactionDbContextServices transactions)
    {
        _userServices = userServices;
        _permissionServices = permissionServices;
        _transactions = transactions;
        expirationActivateCodeTime = Double.Parse(configuration.GetSection("ExpirationActivateCode").Value);
    }

    #endregion

    #region Register

    [Route("/Register")]
    [Route("/Account/Register")]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");

        return View();
    }


    [Route("/Register")]
    [Route("/Account/Register")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto model , CancellationToken cancellationToken)
    {
        #region ValidationInput

        if (!ModelState.IsValid) return View(model);

        //User Exist And Confirmed His Phone
        if (await _userServices.IsUserExistAsync(model.UserName.Trim(), model.PhoneNumber , cancellationToken))
        {
            if (await _userServices.IsPhoneConfirmedAsync(model.PhoneNumber, cancellationToken))
            {
                ViewBag.IsUserExist = true;
                return View(model);
            }
            return Redirect($"/Account/RegisterPhone/{model.PhoneNumber}");
        }

        ////Duplicate UserName & PhoneNumber
        if (await _userServices.IsUserNameOrPhoneNumberExistAsync(model.UserName.Trim(), model.PhoneNumber, cancellationToken))
        {
            ViewBag.CanNotSignUp = true;
            return View(model);
        }

        #endregion

        var user = new User
        {
            UserName = model.UserName.Trim(),
            PhoneNumber = model.PhoneNumber,
            PasswordHash = SecurityHelper.GetSha256Hash(model.Password.Trim()),
            UserAvatar = "Default.png",
            RegisterDate = DateTime.Now,
            PhoneNumberConfirmed = false,
        };

        await _userServices.SignUpUserAsync(user , cancellationToken);

        return Redirect($"/Account/RegisterPhone/{model.PhoneNumber}");
    }

    #endregion

    #region RegisterPhone

    [Route("/Account/RegisterPhone/{phone}/{forgetPassword?}")]
    public async Task<IActionResult> RegisterPhone(string phone, CancellationToken cancellationToken, bool forgetPassword = false)
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");
        if (!await _userServices.IsPhoneExistAsync(phone, cancellationToken)) return NotFound();

        var model = new RegisterPhoneDto
        {
            PhoneNumber = phone,
        };

        var timeExpired = (int)await _userServices.LeftTimeActivateCodeAsync(phone, expirationActivateCodeTime , cancellationToken);
        if (timeExpired > 0)
        {
            model.ExpirationTime = DateTime.Now.AddSeconds(timeExpired);
            ViewBag.ForgetPassword = forgetPassword;
            return View(model);
        }

        var activeCodeForUser = Generator.GenerateUniqCode();

        //TODO Send SMS(totpCode)

        await _userServices.AddActiveCodForUserAsync(phone, activeCodeForUser, cancellationToken);

        ViewBag.ForgetPassword = forgetPassword;
        model.ExpirationTime = DateTime.Now.AddSeconds(expirationActivateCodeTime);

        return View(model);
    }

    [Route("/Account/RegisterPhone/{phone?}/{forgetPassword?}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RegisterPhone(RegisterPhoneDto model, CancellationToken cancellationToken, bool forgetPassword = false)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userServices.GetUserByPhoneNumberAsync(model.PhoneNumber, cancellationToken);
        if (user == null) return NotFound();

        #region Validation Active Code

        if (user.ActiveCode != model.Code)
        {
            ModelState.AddModelError(nameof(model.Code), "کد وارد شده معتبر نمیباشد.");
            ViewBag.ForgetPassword = forgetPassword;
            return View(model);
        }
        else if(forgetPassword) return Redirect($"/Account/ResetPassword/{model.PhoneNumber}");


        if (user.CreateActiveCode.Value.AddSeconds(expirationActivateCodeTime) < DateTime.Now)
        {
            ModelState.AddModelError(nameof(model.Code), "تاریخ مصرف کد تایید گذشته است.لطفا دوباره امتحان کنید.");
            ViewBag.ForgetPassword = forgetPassword;
            return View(model);
        }

        #endregion Validation Active Code

        user.PhoneNumberConfirmed = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        #region SignInUser

        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name , user.UserName),
            };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        #endregion

        return Redirect("/Home/SuccesSign");
    }

    #endregion

    #region Login

    [Route("/Login")]
    [Route("/Account/Login")]
    public IActionResult Login(string? ReturnUrl, bool ChangePassword = false)
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");

        ViewData["ReturnUrl"] = ReturnUrl;
        ViewBag.ChangePassword = ChangePassword;

        return View();
    }

    [Route("/Login")]
    [Route("/Account/Login")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model, string? ReturnUrl, CancellationToken cancellationToken)
    {
        #region ValidationUser

        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(model);
        }
        var user = await _userServices.CheckUserForLoginAsync(model.UserName,model.Password, cancellationToken);
        if (user == null)
        {
            ViewBag.IsNotExist = true;
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(model);
        }
        if (!await _userServices.IsPhoneConfirmedAsync(user.PhoneNumber, cancellationToken))
        {
            ViewBag.IsNotConfirmedPhone = true;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(model);
        }

        #endregion ValidationUser

        #region SignInUser

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name , user.UserName),
            };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var properties = new AuthenticationProperties()
        {
            IsPersistent = model.RememberMe
        };
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal, properties);

        #endregion

        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)) return Redirect(ReturnUrl);

        return Redirect("/Home/SuccessSign");
    }

    #endregion

    #region LogOut

    [Route("/LogOut")]
    [Route("/Account/LogOut")]
    public async Task<IActionResult> LogOut()
    {
        if (!User.Identity.IsAuthenticated) return Redirect("/");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Home");
    }

    #endregion

    #region ForgetPassword

    [Route("/Account/ForgetPassword")]
    [Route("/ForgetPassword")]
    public IActionResult ForgetPassword()
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");
        return View();
    }

    [Route("/Account/ForgetPassword")]
    [Route("/ForgetPassword")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model , CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (!await _userServices.IsPhoneExistAsync(model.PhoneNumber, cancellationToken) || 
            !await _userServices.IsPhoneConfirmedAsync(model.PhoneNumber, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.PhoneNumber), "شماره تلفن وارد شده معتبر نیست.");
            return View(model);
        }

        return Redirect($"/Account/RegisterPhone/{model.PhoneNumber}/true");
    }

    #endregion

    #region ResetPassword

    [Route("/Account/ResetPassword/{phoneNumber}")]
    public async Task<IActionResult> ResetPassword(string phoneNumber , CancellationToken cancellationToken)
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");

        var user = await _userServices.GetUserByPhoneNumberAsync(phoneNumber , cancellationToken , false);
        if (user == null || !await _userServices.IsPhoneConfirmedAsync(user.PhoneNumber, cancellationToken)) return NotFound();

        var model = new ResetPasswordDto() { UserId = user.UserId };

        return View(model);
    }

    [Route("/Account/ResetPassword")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userServices.GetUserByIdAsync(model.UserId, cancellationToken);
        if (user == null) return NotFound();

        var newHashPass = SecurityHelper.GetSha256Hash(model.NewPassword.Trim());

        if (user.PasswordHash == newHashPass)
        {
            ModelState.AddModelError(nameof(model.NewPassword), "رمز عبور وارد شده قبلا استفاده شده است.");
            return View(model);
        }

        user.PasswordHash = newHashPass;
        await _transactions.SaveChangesAsync(cancellationToken);

        #region SignInUser

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name , user.UserName),
            };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var properties = new AuthenticationProperties()
        {
            IsPersistent = model.RememberMe
        };
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal, properties);

        #endregion

        return Redirect("/Home/SuccessSign");
    }

    #endregion
}
