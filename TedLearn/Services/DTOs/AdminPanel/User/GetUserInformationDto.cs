using Services.DTOs.Common;

namespace Services.DTOs.AdminPanel.User;

public class GetUserInformationDto : BaseDto<GetUserInformationDto , Data.Entities.Persons.Users.User>
{
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public decimal Stock { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool PhoneNumberConfirmed { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime RegisterDate { get; set; }
    public string? WebsiteAddress { get; set; }
    public string? Biography { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Persons.Users.User, GetUserInformationDto> mapping)
    {
        mapping.ForMember(dto => dto.Stock, opt => opt.Ignore());
    }
}
