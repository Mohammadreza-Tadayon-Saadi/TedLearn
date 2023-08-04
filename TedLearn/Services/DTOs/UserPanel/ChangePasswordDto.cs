namespace Services.DTOs.UserPanel;

public class ChangePasswordDto
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ReNewPassword { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
