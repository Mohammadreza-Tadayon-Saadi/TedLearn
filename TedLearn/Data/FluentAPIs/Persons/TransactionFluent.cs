using Data.Entities.Persons.Transactions;

namespace Data.FluentAPIs.Persons;

public class TransactionFluent : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasCheckConstraint("CK_Amount", "[Amount] >= 1000 And [Amount] <= 2500000");
    }
}
