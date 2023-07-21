using Data.Entities.Persons.Roles;

namespace Services.DTOs.AdminPanel.Role;

public class UserRoleDto : BaseDto<UserRoleDto, UserRole>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<UserRole, UserRoleDto> mapping)
    {
        mapping.ForMember(dto => dto.RoleName, opt => opt.MapFrom(ur => ur.Role.RoleName));
    }
}
