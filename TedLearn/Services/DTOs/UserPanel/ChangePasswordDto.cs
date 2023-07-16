namespace Services.DTOs.UserPanel;

public class ChangePasswordDto
{
    [Display(Name = "رمز عبور فعلی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public string Password { get; set; } = string.Empty;


    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(4, ErrorMessage = "{0} باید بیشتر از {1} کارکتر باشد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Display(Name = "تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Compare(nameof(NewPassword), ErrorMessage = "{0} با رمز عبور وارد شده همخوانی ندارد.")]
    [MaxLength(40, ErrorMessage = "{0} نباید بیشتر از {1} باشد.")]
    [DataType(DataType.Password)]
    public string ReNewPassword { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
