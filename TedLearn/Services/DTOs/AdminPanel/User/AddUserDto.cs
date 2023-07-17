using Services.DTOs.Account;

namespace Services.DTOs.AdminPanel.User;

public class AddUserDto : RegisterDto
{
    public bool PhoneNumberConfirmed { get; set; }
}
