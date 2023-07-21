namespace Services.DTOs.AdminPanel.Course;

public class ShowCourseDto : BaseDto<ShowCourseDto , Data.Entities.Products.Courses.Course>
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.Course, ShowCourseDto> mapping)
    {
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(c => c.CourseTitle));
        mapping.ForMember(dto => dto.Price, opt => opt.MapFrom(c => c.CoursePrice));
        mapping.ForMember(dto => dto.Image, opt => opt.MapFrom(c => c.CourseImage));
        mapping.ForMember(dto => dto.TeacherName, opt => opt.MapFrom(c => c.User.UserName));
    }
}
