using Data.Entities.Persons.Users;
using Microsoft.AspNetCore.Http;

namespace Services.DTOs.UserPanel.Home;

public class UserInformationDto : BaseDto<UserInformationDto, User>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string UserAvatar { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Biography { get; set; }
    public string? WebsiteAddress { get; set; }
    public IFormFile Image { get; set; }

    public override void CustomMappings(IMappingExpression<User, UserInformationDto> mapping)
    {
        mapping.ForMember(dto => dto.FullName, opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"));
    }
}

