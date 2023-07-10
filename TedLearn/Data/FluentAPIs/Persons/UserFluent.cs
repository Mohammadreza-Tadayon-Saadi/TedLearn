using Data.Entities.Persons.Users;

namespace Data.FluentAPIs.Persons;

public class UserFluent : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(p => p.UserName)
                   .IsUnique()
                   .HasDatabaseName("IX_Users_UserName");

        builder.HasIndex(p => p.PhoneNumber)
               .IsUnique()
               .HasDatabaseName("IX_Users_PhoneNumber");

        builder.Property(cg => cg.UserId).HasAnnotation("SqlServer:Identity", "11001,1");

        builder.Property(p => p.Version).HasComment("This Column Is For Concurrency Check");
    }
}
