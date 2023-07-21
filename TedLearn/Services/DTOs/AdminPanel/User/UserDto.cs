using Services.DTOs.AdminPanel.Role;

namespace Services.DTOs.AdminPanel.User;

public class UserDto : BaseDto<UserDto , Data.Entities.Persons.Users.User>
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public bool IsDelete { get; set; }
    public DateTime RegisterDate { get; set; }

    public List<UserRoleDto> UserRoles { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Persons.Users.User, UserDto> mapping)
    {
        mapping.ForMember(dto => dto.UserRoles, opt => opt.MapFrom(u => u.UserRole));
    }
}
