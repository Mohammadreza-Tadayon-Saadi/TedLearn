namespace Services.DTOs.Account;

public class ResetPasswordDto
{
    public int UserId { get; set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ReNewPassword { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}
