using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course;

public class AddCourseDto
{
    public string CourseTitle { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public int? SubGroupId { get; set; }
    public int TeacherId { get; set; }
    public int StatusId { get; set; }
    public string LevelId { get; set; } = string.Empty;
    public decimal? CoursePrice { get; set; }
    public IFormFile DemoFile { get; set; }
    public IFormFile ImageFile { get; set; }
    public string? CourseRequirment { get; set; }
    public string? Tags { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public IEnumerable<SelectListItem>? GroupList { get; set; }
    public IEnumerable<SelectListItem>? TeacherList { get; set; }
    public IEnumerable<SelectListItem>? StatusList { get; set; }
}
