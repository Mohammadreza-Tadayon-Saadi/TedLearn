namespace Services.DTOs.AdminPanel.Role;

public class GetRolesForUserDto
{
    public IEnumerable<UserRoleDto> UserIsInRoles { get; set; }
    public IEnumerable<UserRoleDto> UserIsNotInRoles { get; set; }
}
