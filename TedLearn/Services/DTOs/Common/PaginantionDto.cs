namespace Services.DTOs.Common;

public class PaginantionDto
{
    public int Currentpage { get; set; }
    public int PageCount { get; set; }
    public int ItemsCount { get; set; }
    public int ItemsPerPage { get; set; }
}
