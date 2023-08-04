using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course.CourseGroup;

public class AddSubGroupDto
{
    public int? ParentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<SelectListItem>? GroupList { get; set; }
}
