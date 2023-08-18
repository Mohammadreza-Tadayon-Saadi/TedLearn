namespace Services.DTOs.Home.Course;

public class ShowCourseCardDto : BaseDto<ShowCourseCardDto , Data.Entities.Products.Courses.Course>
{
    public int CourseId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string CourseImage { get; set; } = string.Empty;
    public decimal CoursePrice { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int CountOfUser { get; set; }
    public int Discount { get; set; }
    public DateTime CreateDate { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.Course, ShowCourseCardDto> mapping)
    {
        mapping.ForMember(dto => dto.GroupName, opt => opt.MapFrom(c => c.CourseToGroup.Title));
        mapping.ForMember(dto => dto.TeacherName, opt => opt.MapFrom(c => c.User.UserName));
        mapping.ForMember(dto => dto.CountOfUser, opt => opt.MapFrom(c => c.UserCourses.Count()));
        mapping.ForMember(dto => dto.Discount, opt => opt.MapFrom(c => 0));
    }
}
