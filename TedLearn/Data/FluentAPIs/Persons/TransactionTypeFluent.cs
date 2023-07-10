using Data.Entities.Persons.Transactions;

namespace Data.FluentAPIs.Persons;

public class TransactionTypeFluent : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasData
        (
            new TransactionType()
            {
                TypeId = 1,
                TypeTitle = "واریز"
            },
            new TransactionType()
            {
                TypeId = 2,
                TypeTitle = "برداشت"
            }
        );
    }
}
