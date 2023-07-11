namespace WebConfig.DTOs.Account;

public class RegisterPhoneDto
{
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "کد فعالسازی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "تاریخ انقضا")]
    public DateTime ExpirationTime { get; set; }
}
