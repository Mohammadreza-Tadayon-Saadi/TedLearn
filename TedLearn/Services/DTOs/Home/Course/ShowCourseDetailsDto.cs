using Data.Entities.Products.Courses;

namespace Services.DTOs.Home.Course;

public class ShowCourseDetailsDto : BaseDto<ShowCourseDetailsDto , Data.Entities.Products.Courses.Course>
{
    #region Course

    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public decimal CoursePrice { get; set; }
    public string StatusTitle { get; set; } = string.Empty;
    public string CourseLevel { get; set; } = string.Empty;
    public DateTime LastUpdateDate { get; set; }
    public string CourseTags { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public string CourseRequirement { get; set; } = string.Empty;
    public string CourseDemoFile { get; set; } = string.Empty;
    public string CourseImage { get; set; } = string.Empty;

    #endregion


    #region Teacher

    public int TeacherId { get; set; }
    public string TeacherUserName { get; set; } = string.Empty;
    public string TeacherLastName { get; set; } = string.Empty;
    public string TeacherProfile { get; set; } = string.Empty;
    public string TeacherBio { get; set; } = string.Empty;
    public string TeacherWebsite { get; set; } = string.Empty;

    #endregion


    public IEnumerable<CourseSeason> CourseSeasons { get; set; }

    public string GroupName { get; set; } = string.Empty;
    public string? SubGroupName { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.Course, ShowCourseDetailsDto> mapping)
    {
        mapping.ForMember(dto => dto.StatusTitle, opt => opt.MapFrom(c => c.CourseStatus.StatusTitle));
        mapping.ForMember(dto => dto.GroupName, opt => opt.MapFrom(c => c.CourseToGroup.Title));
        mapping.ForMember(dto => dto.SubGroupName, opt => opt.MapFrom(c => c.CourseToSubGroup.Title));
        mapping.ForMember(dto => dto.TeacherUserName, opt => opt.MapFrom(c => c.User.UserName));
        mapping.ForMember(dto => dto.TeacherLastName, opt => opt.MapFrom(c => c.User.LastName));
        mapping.ForMember(dto => dto.TeacherProfile, opt => opt.MapFrom(c => c.User.UserAvatar));
        mapping.ForMember(dto => dto.TeacherUserName, opt => opt.MapFrom(c => c.User.Biography));
        mapping.ForMember(dto => dto.TeacherWebsite, opt => opt.MapFrom(c => c.User.WebsiteAddress));
    }
}
