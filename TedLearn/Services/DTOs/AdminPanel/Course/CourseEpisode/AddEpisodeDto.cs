using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course.CourseEpisode;

public class AddEpisodeDto
{
    public string Title { get; set; } = string.Empty;
    public TimeSpan Time { get; set; }
    public IFormFile EpisodeFile { get; set; }
    public int SeasonId { get; set; }
    public int CourseId { get; set; }
    public bool IsFree { get; set; }

    public IEnumerable<SelectListItem>? SeasonList { get; set; }
}
