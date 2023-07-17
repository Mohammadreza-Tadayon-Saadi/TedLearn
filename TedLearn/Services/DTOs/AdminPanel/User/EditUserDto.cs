using Services.DTOs.Common;

namespace Services.DTOs.AdminPanel.User;

public class EditUserDto : BaseDto<EditUserDto ,Data.Entities.Persons.Users.User>
{
    public int UserId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool PhoneNumberConfirmed { get; set; }
    public string? Password { get; set; }
    public decimal? Amount { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Biography { get; set; }
    public string? WebsiteAddress { get; set; }
    public string Base64Version { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Persons.Users.User, EditUserDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(u => Convert.ToBase64String(u.Version)));
        mapping.ForMember(dto => dto.Amount, opt => opt.Ignore());
    }
}
