using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;
using Data.Entities.Products.Comment;
using Data.Entities.Products.Question;

namespace Data.Entities.Products.Courses;

[Table("Courses" , Schema = "Products")]
public class Course : IEntity
{
    #region Keys

    [Key]
    public int CourseId { get; set; }
    public int GroupId { get; set; }
    public int? SubGroupId { get; set; }
    public int TeacherId { get; set; }
    public int StatusId { get; set; }

    #endregion

    #region Column

    [MaxLength(20)]
    public string CourseLevel { get; set; } = string.Empty;

    [MaxLength(80)]
    public string CourseTitle { get; set; } = string.Empty;

    [Column(TypeName = "money")]
    public decimal CoursePrice { get; set; }

    [MaxLength(150)]
    public string CourseDemoFile { get; set; } = string.Empty;

    [MaxLength(150)]
    public string CourseImage { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string CourseDescription { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? CourseRequirement { get; set; }

    [MaxLength(500)]
    public string? CourseTags { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }

    #endregion

    #region Relations

    [ForeignKey(nameof(TeacherId))]
    public User User { get; set; }

    [ForeignKey(nameof(StatusId))]
    public CourseStatus CourseStatus { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public CourseGroup CourseToGroup { get; set; }

    [ForeignKey(nameof(SubGroupId))]
    public CourseGroup CourseToSubGroup { get; set; }
    public ICollection<UserCourse> UserCourses { get; set; }
    public ICollection<CourseSeason> CourseSeasons { get; set; }
    public ICollection<CourseComment> CourseComments { get; set; }
    public ICollection<AnswerQuestion> AnswerQuestions { get; set; }

    #endregion
}
