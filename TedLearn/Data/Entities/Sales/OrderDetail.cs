using Data.Entities.BaseEntity;
using Data.Entities.Products.Courses;

namespace Data.Entities.Sales;

[Table("OrderDetails" , Schema = "Sales")]
public class OrderDetail : IEntity
{
    [Key]
    public int DetailId { get; set; }
    public int OrderId { get; set; }
    public int CourseId { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    #region Relations

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }

    #endregion Relations
}
