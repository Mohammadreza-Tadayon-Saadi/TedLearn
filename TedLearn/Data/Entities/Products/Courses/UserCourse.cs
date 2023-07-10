using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;

namespace Data.Entities.Products.Courses;

[Table("UserCourses", Schema = "Products")]
public class UserCourse : IEntity
{
    [Key]
    public int UC_Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }

    #region Relations

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }

    #endregion
}
