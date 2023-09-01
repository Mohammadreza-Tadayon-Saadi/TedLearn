using Data.Entities.Sales;

namespace Services.DTOs.UserPanel.Order;

public class GetOrderDetailsForUserPanelDto : BaseDto<GetOrderDetailsForUserPanelDto , OrderDetail>
{
    public decimal OrderDetailDiscounts { get; set; } = 0;
    public decimal Price { get; set; }
    public string CourseTitle { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<OrderDetail, GetOrderDetailsForUserPanelDto> mapping)
    {
        mapping.ForMember(dto => dto.CourseTitle, opt => opt.MapFrom(od => od.Course.CourseTitle));
    }
}
