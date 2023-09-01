using Core.Convertors;
using Core.Utilities;
using Data.Entities.Persons.Discounts;

namespace Services.DTOs.AdminPanel.UserDiscount;

public class UserDiscountDto : BaseDto<UserDiscountDto , UDiscount> 
{
    public int? DiscountId { get; set; }
    public string DiscountCode { get; set; } = string.Empty;
    public byte Percent { get; set; }
    public Int16 UsableCount { get; set; }
    public string StartDateShamsi { get; set; } = string.Empty;
    public string EndDateShamsi { get; set; } = string.Empty;
    public string Base64Version { get; set; } = string.Empty;
    public bool IsDelete { get; set; }

    public override void CustomMappings(IMappingExpression<UDiscount, UserDiscountDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(ud => ud.Version.ToBase64String()));
        mapping.ForMember(dto => dto.StartDateShamsi, opt => opt.MapFrom(ud => ud.StartDate.MiladiToShamsi(true)));
        mapping.ForMember(dto => dto.EndDateShamsi, opt => opt.MapFrom(ud => ud.EndDate.MiladiToShamsi(true)));
    }
}
