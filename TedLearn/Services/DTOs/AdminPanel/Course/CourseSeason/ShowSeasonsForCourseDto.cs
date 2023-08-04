namespace Services.DTOs.AdminPanel.Course.CourseSeason;

public class ShowSeasonsForCourseDto : BaseDto<ShowSeasonsForCourseDto,Data.Entities.Products.Courses.CourseSeason>
{
    public int SeasonId { get; set; }
    public int CourseId { get; set; }

    public bool IsDelete { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseSeason, ShowSeasonsForCourseDto> mapping)
    {
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(cs => cs.SeasonTitle));
    }
}
