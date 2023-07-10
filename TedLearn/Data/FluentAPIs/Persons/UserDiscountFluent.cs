using Data.Entities.Persons.Discounts;

namespace Data.FluentAPIs.Persons;

public class UserDiscountFluent : IEntityTypeConfiguration<UserDiscount>
{
    public void Configure(EntityTypeBuilder<UserDiscount> builder)
    {
        builder.HasCheckConstraint("CK_UserDiscounts_Percent", "[Percent] > 0 And [Percent] <= 100");
    }
}
