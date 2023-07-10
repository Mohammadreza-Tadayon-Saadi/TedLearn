using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;

namespace Data.Entities.Persons.Discounts;

[Table("UserDiscounts", Schema = "Persons")]
public class UserDiscount : IEntity
{
    [Key]
    public int UserDiscountId { get; set; }
    public int UserId { get; set; }
    public int DiscountId { get; set; }
    public byte Percent { get; set; }
    public DateTime UseDate { get; set; }

    #region Relations

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(DiscountId))]
    public UDiscount UDiscount { get; set; }

    #endregion
}
