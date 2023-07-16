namespace Services.DTOs.Account;

public class RegisterPhoneDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime ExpirationTime { get; set; }
}
