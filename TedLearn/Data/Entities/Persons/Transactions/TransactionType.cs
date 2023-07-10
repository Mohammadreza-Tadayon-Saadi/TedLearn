using Data.Entities.BaseEntity;

namespace Data.Entities.Persons.Transactions;

[Table("TransactionTypes", Schema = "Persons")]
public class TransactionType : IEntity
{
    [Key]
    public int TypeId { get; set; }

    [MaxLength(60)]
    public string TypeTitle { get; set; } = string.Empty;

    #region Relations

    public ICollection<Transaction> Transactions { get; set; }

    #endregion
}
