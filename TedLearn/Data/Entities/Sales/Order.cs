using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;

namespace Data.Entities.Sales;

[Table("Orders", Schema = "Sales")]
public class Order : IEntity
{
    [Key]
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public bool IsFinaly { get; set; }
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "money")]
    public decimal Discount { get; set; }

    #region Relations

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }

    #endregion Relations
}
