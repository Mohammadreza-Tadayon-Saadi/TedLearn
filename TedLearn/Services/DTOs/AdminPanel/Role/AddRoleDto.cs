namespace Services.DTOs.AdminPanel.Role;

public class AddRoleDto
{
    public string RoleName { get; set; } = string.Empty;
    public bool CanDeleteOrEdit { get; set; }
}
