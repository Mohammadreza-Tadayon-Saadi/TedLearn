using Core.Utilities;
using Data.Entities.Persons.Users;

namespace Services.DTOs.UserPanel;

public class EditUserProfileDto : BaseDto<EditUserProfileDto, User>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Biography { get; set; }
    public string? WebsiteAddress { get; set; }
    public string UserAvatar { get; set; } = string.Empty;
    public string Base64Version { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<User, EditUserProfileDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(u => u.Version.ToBase64String()));
    }
}
