using Data.Entities.Sales;

namespace Services.DTOs.Home.Order;

public class DetailsBasketDto : BaseDto<DetailsBasketDto , OrderDetail>
{
    public int DetailId { get; set; }
    public int CourseId { get; set; }
    public decimal Price { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string CourseImage { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<OrderDetail, DetailsBasketDto> mapping)
    {
        mapping.ForMember(dto => dto.CourseTitle, opt => opt.MapFrom(od => od.Course.CourseTitle));
        mapping.ForMember(dto => dto.CourseImage, opt => opt.MapFrom(od => od.Course.CourseImage));
        mapping.ForMember(dto => dto.TeacherName, opt => opt.MapFrom(od => od.Course.User.UserName));
    }
}
