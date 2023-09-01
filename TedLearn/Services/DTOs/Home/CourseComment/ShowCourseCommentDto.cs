namespace Services.DTOs.Home.CourseComment;

public class ShowCourseCommentDto
{
    public PaginantionDto Paginantion { get; set; }
    public IEnumerable<ShowCourseCommentDetailsDto> Comments { get; set; }
}
