using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Dto.AdminPanel.Course.CourseEpisode;

public class EditEpisodeDto : BaseDto<EditEpisodeDto , Data.Entities.Products.Courses.CourseEpisode>
{
    public int EpisodeId { get; set; }
    public int SeasonId { get; set; }
    public int CourseId { get; set; }

    public string Title { get; set; } = string.Empty;
    public TimeSpan Time { get; set; }
    public bool IsFree { get; set; }
    public string Base64Version { get; set; } = string.Empty;
    public string PreEpisodeFileName { get; set; } = string.Empty;

    public IFormFile? File { get; set; }
    public IEnumerable<SelectListItem>? SeasonList { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseEpisode, EditEpisodeDto> mapping)
    {
        mapping.ForMember(dto => dto.CourseId, opt => opt.MapFrom(ce => ce.CourseSeason.CourseId));
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(ce => ce.EpisodeTitle));
        mapping.ForMember(dto => dto.Time, opt => opt.MapFrom(ce => ce.EpisodeTime));
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(ce => ce.Version.ToBase64String()));
        mapping.ForMember(dto => dto.PreEpisodeFileName, opt => opt.MapFrom(ce => ce.EpisodeFile));
    }
}
