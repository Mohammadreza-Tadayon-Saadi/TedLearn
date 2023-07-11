namespace WebConfig.DTOs.Account;

public class ResetPasswordDto
{
    public int UserId { get; set; }

    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(4, ErrorMessage = "{0} باید بیشتر از {1} کارکتر باشد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    public string NewPassword { get; set; } = string.Empty;

    [Display(Name = "تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Compare("NewPassword", ErrorMessage = "{0} با رمز عبور وارد شده همخوانی ندارد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    public string ReNewPassword { get; set; } = string.Empty;

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}
