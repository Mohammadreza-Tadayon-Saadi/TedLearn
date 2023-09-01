namespace Services.DTOs.UserPanel.Order;

public class GetMyCoursesDto
{
    public PaginantionDto Paginantion { get; set; }
    public IEnumerable<ShowMyCoursesDto> MyCourses { get; set; }
}
