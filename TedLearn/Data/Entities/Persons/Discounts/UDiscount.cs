using Data.Entities.BaseEntity;

namespace Data.Entities.Persons.Discounts;

[Table("Discounts", Schema = "Persons")]
public class UDiscount : IEntity
{
    [Key]
    public int DiscountId { get; set; }

    [MaxLength(80)]
    public string DiscountCode { get; set; } = string.Empty;
    public byte Percent { get; set; }
    public Int16 UsableCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }

    #region Relations

    public ICollection<UserDiscount> UserDiscounts { get; set; }

    #endregion
}
