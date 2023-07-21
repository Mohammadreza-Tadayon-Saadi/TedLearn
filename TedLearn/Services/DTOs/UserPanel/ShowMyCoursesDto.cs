namespace Services.DTOs.UserPanel;

public class ShowMyCoursesDto
{
    public int UC_Id { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string? TeacherWebsite { get; set; }
    public DateTime LastOfUpdate { get; set; }
}
