using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;
using Data.Entities.Products.Courses;

namespace Data.Entities.Products.Comment;

[Table("CourseComments", Schema = "Products")]
public class CourseComment : IEntity
{
    [Key]
    public int CommentId { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }


    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }


    #region Relations

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(ParentId))]
    public ICollection<CourseComment> ReplyComment { get; set; }

    #endregion
}
