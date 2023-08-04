using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course.CourseSeason;

public class AddSeasonDto
{
    public string Title { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public IEnumerable<SelectListItem>? CourseList { get; set; }
}
