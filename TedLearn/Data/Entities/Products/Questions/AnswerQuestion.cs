using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;
using Data.Entities.Products.Courses;

namespace Data.Entities.Products.Question;

[Table("AnswerQuestions", Schema = "Products")]
public class AnswerQuestion : IEntity
{
    [Key]
    public int AQ_Id { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public int? AnswerId { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool HasRightAnswer { get; set; }
    public bool IsDelete { get; set; }


    #region Relations

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(AnswerId))]
    public ICollection<AnswerQuestion> Answers { get; set; }

    #endregion
}
