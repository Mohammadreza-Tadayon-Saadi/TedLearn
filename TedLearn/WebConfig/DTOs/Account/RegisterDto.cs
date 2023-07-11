namespace WebConfig.DTOs.Account;

public class RegisterDto
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(3, ErrorMessage = "{0} باید بیشتر از {1} کارکتر باشد.")]
    [MaxLength(80, ErrorMessage = "{0} نباید بیشتر از {1} کارکتر باشد.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [RegularExpression(@"^09[0|1|2|3|4|9][0-9]{8}$", ErrorMessage = "{0} وارد شده صحیح نیست.")]
    [MinLength(11, ErrorMessage = "{0} وارد شده صحیح نیست.")]
    [MaxLength(11, ErrorMessage = "{0} وارد شده صحیح نیست.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(4, ErrorMessage = "{0} باید بیشتر از {1} کارکتر باشد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Compare(nameof(Password) , ErrorMessage = "{0} با رمز عبور وارد شده همخوانی ندارد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    public string RePassword { get; set; } = string.Empty;
}
