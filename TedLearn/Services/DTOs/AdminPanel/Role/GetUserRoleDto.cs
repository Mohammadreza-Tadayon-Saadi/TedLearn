namespace Services.DTOs.AdminPanel.Role;

public class GetUserRoleDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
