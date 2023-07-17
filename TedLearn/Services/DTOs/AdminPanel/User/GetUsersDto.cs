namespace Services.DTOs.AdminPanel.User;

public class GetUsersDto
{
    public List<UserDto> Users { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
}
