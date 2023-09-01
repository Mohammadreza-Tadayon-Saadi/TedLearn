using Data.Entities.Products.Courses;

namespace Services.DTOs.UserPanel.Order;

public class ShowMyCoursesDto : BaseDto<ShowMyCoursesDto , UserCourse>
{
    public int UC_Id { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string? TeacherWebsite { get; set; }
    public DateTime LastOfUpdate { get; set; }

    public override void CustomMappings(IMappingExpression<UserCourse, ShowMyCoursesDto> mapping)
    {
        mapping.ForMember(dto => dto.CourseTitle, opt => opt.MapFrom(uc => uc.Course.CourseTitle));
        mapping.ForMember(dto => dto.LastOfUpdate, opt => opt.MapFrom(uc => uc.Course.LastUpdateDate));
        mapping.ForMember(dto => dto.TeacherName, opt => opt.MapFrom(uc => uc.Course.User.UserName));
        mapping.ForMember(dto => dto.TeacherWebsite, opt => opt.MapFrom(uc => uc.Course.User.WebsiteAddress));
    }
}
