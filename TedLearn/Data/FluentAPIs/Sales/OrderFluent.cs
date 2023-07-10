using Data.Entities.Sales;

namespace Data.FluentAPIs.Sales;

public class OrderFluent : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(p => p.Discount).HasDefaultValue(0);
    }
}
