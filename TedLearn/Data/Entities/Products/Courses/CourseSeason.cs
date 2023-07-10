using Data.Entities.BaseEntity;

namespace Data.Entities.Products.Courses;

[Table("CourseSeasons", Schema = "Products")]
public class CourseSeason : IEntity
{
    [Key]
    public int SeasonId { get; set; }
    public int CourseId { get; set; }

    [MaxLength(80)]
    public string SeasonTitle { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }

    #region Relation

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }
    public ICollection<CourseEpisode> CourseEpisodes { get; set; }

    #endregion
}
