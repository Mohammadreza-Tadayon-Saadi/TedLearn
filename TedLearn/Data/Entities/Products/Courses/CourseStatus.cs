using Data.Entities.BaseEntity;

namespace Data.Entities.Products.Courses;

[Table("CourseStatuses", Schema = "Products")]
public class CourseStatus : IEntity
{
    [Key]
    public int StatusId { get; set; }

    [MaxLength(40)]
    public string StatusTitle { get; set; } = string.Empty;


    #region Relations

    public ICollection<Course> Courses { get; set; }

    #endregion
}
