using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.DTOs.Home.Course;

public class FilteredCoursesDto
{
    public PaginantionDto Paginantion { get; set; }
    public IEnumerable<ShowCourseCardDto> CourseCards { get; set; }
    public IEnumerable<ShowCourseGroupDto> CourseGroups { get; set; }
}
