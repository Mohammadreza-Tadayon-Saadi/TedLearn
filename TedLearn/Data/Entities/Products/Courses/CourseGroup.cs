using Data.Entities.BaseEntity;

namespace Data.Entities.Products.Courses;

[Table("CourseGroups", Schema = "Products")]
public class CourseGroup : IEntity
{
    [Key]
    public int GroupId { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    public int? SubGroupId { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }

    #region Relations

    [ForeignKey(nameof(SubGroupId))]
    public ICollection<CourseGroup> SubCourseGroups { get; set; }

    [InverseProperty("CourseToGroup")]
    public ICollection<Course> Group { get; set; }
    
    [InverseProperty("CourseToSubGroup")]
    public ICollection<Course> SubGroup { get; set; }

    #endregion
}
