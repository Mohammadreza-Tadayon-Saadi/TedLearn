namespace Services.DTOs.AdminPanel.Course.CourseGroup;

public class ShowCourseGroupDto : BaseDto<ShowCourseGroupDto , Data.Entities.Products.Courses.CourseGroup>
{
    public int GroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDelete { get; set; }
    public int? SubGroupId { get; set; }
}
