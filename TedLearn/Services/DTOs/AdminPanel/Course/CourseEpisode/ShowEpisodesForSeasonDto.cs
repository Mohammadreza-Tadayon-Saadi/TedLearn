namespace Services.DTOs.AdminPanel.Course.CourseEpisode;

public class ShowEpisodesForSeasonDto : BaseDto<ShowEpisodesForSeasonDto , Data.Entities.Products.Courses.CourseEpisode>
{
    public int EpisodeId { get; set; }
    public int SeasonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsFree { get; set; }
    public TimeSpan Time { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseEpisode, ShowEpisodesForSeasonDto> mapping)
    {
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(ce => ce.EpisodeTitle));
        mapping.ForMember(dto => dto.Time, opt => opt.MapFrom(ce => ce.EpisodeTime));
    }
}
