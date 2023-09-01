namespace Services.DTOs.Home.CourseEpisode;

public class GetEpisodeForShowOnlineDto : BaseDto<GetEpisodeForShowOnlineDto , Data.Entities.Products.Courses.CourseEpisode>
{
    public string EpisodeFile { get; set; } = string.Empty;
    public bool IsFree { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public string CourseImage { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseEpisode, GetEpisodeForShowOnlineDto> mapping)
    {
        mapping.ForMember(dto => dto.CourseId, opt => opt.MapFrom(ce => ce.CourseSeason.CourseId));
        mapping.ForMember(dto => dto.CourseImage, opt => opt.MapFrom(ce => ce.CourseSeason.Course.CourseImage));
        mapping.ForMember(dto => dto.TeacherId, opt => opt.MapFrom(ce => ce.CourseSeason.Course.TeacherId));
    }
}
