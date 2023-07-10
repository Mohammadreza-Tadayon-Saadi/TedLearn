using Data.Entities.BaseEntity;

namespace Data.Entities.Products.Courses;

[Table("CourseEpisodes", Schema = "Products")]
public class CourseEpisode : IEntity
{
    [Key]
    public int EpisodeId { get; set; }
    public int SeasonId { get; set; }

    [MaxLength(80)]
    public string EpisodeTitle { get; set; } = string.Empty;

    [MaxLength(150)]
    public string EpisodeFile { get; set; } = string.Empty;
    public TimeSpan EpisodeTime { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsFree { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }

    #region Relation

    [ForeignKey(nameof(SeasonId))]
    public CourseSeason CourseSeason { get; set; }

    #endregion
}
