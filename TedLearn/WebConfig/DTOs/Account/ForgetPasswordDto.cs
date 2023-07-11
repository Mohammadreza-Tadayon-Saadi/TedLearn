namespace WebConfig.DTOs.Account;

public class ForgetPasswordDto
{
    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [RegularExpression(@"^09[0|1|2|3|9][0-9]{8}$", ErrorMessage = "{0} وارد شده صحیح نیست.")]
    [MinLength(11, ErrorMessage = "{0} وارد شده صحیح نیست.")]
    [MaxLength(11, ErrorMessage = "{0} وارد شده صحیح نیست.")]
    public string PhoneNumber { get; set; } = string.Empty;
}
