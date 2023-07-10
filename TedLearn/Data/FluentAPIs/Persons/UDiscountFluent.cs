using Data.Entities.Persons.Discounts;

namespace Data.FluentAPIs.Persons;

public class UDiscountFluent : IEntityTypeConfiguration<UDiscount>
{
    public void Configure(EntityTypeBuilder<UDiscount> builder)
    {
        builder.HasIndex(p => p.DiscountCode)
                  .IsUnique()
                  .HasDatabaseName("IX_Discounts_DiscountCode");

        builder.HasCheckConstraint("CK_Discounts_Percent", "[Percent] > 0 And [Percent] <= 100");
        builder.HasCheckConstraint("CK_UsableCount", "UsableCount > 0");
    }
}
