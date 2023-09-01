namespace Services.DTOs.Home.CourseEpisode;

public class GetEpisodeForDownloadDto : BaseDto<GetEpisodeForDownloadDto , Data.Entities.Products.Courses.CourseEpisode>
{
    public string EpisodeFile { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public bool IsFree { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseEpisode, GetEpisodeForDownloadDto> mapping)
    {
        mapping.ForMember(dto => dto.TeacherId, opt => opt.MapFrom(ce => ce.CourseSeason.Course.TeacherId));
    }
}
