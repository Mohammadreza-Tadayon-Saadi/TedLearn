namespace Services.DTOs.AdminPanel.Course;

public class ShowDetailsCourseDto : BaseDto<ShowDetailsCourseDto , Data.Entities.Products.Courses.Course>
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string SubGroupName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public string LevelName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastOfUpdate { get; set; }
    public string Requirement { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.Course, ShowDetailsCourseDto> mapping)
    {
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(c => c.CourseTitle));
        mapping.ForMember(dto => dto.Image, opt => opt.MapFrom(c => c.CourseImage));
        mapping.ForMember(dto => dto.GroupName, opt => opt.MapFrom(c => c.CourseToGroup.Title));
        mapping.ForMember(dto => dto.SubGroupName, opt => opt.MapFrom(c => c.CourseToSubGroup.Title));
        mapping.ForMember(dto => dto.TeacherName, opt => opt.MapFrom(c => c.User.UserName));
        mapping.ForMember(dto => dto.StatusName, opt => opt.MapFrom(c => c.CourseStatus.StatusTitle));
        mapping.ForMember(dto => dto.LevelName, opt => opt.MapFrom(c => c.CourseLevel));
        mapping.ForMember(dto => dto.Price, opt => opt.MapFrom(c => c.CoursePrice));
        mapping.ForMember(dto => dto.LastOfUpdate, opt => opt.MapFrom(c => c.LastUpdateDate));
        mapping.ForMember(dto => dto.Requirement, opt => opt.MapFrom(c => c.CourseRequirement));
        mapping.ForMember(dto => dto.Tags, opt => opt.MapFrom(c => c.CourseTags));
        mapping.ForMember(dto => dto.Description, opt => opt.MapFrom(c => c.CourseDescription));
    }
}
