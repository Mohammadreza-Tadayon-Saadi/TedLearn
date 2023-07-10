using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;

namespace Data.Entities.Persons.Transactions;

[Table("Transactions", Schema = "Persons")]
public class Transaction : IEntity
{
    [Key]
    public int TransactionId { get; set; }

    public int TypeId { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    [MaxLength(150)]
    public string Description { get; set; } = string.Empty;
    public bool IsPay { get; set; }


    #region Relations

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(TypeId))]
    public TransactionType TransactionType { get; set; }

    #endregion
}
