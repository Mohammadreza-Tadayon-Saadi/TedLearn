using Data.Entities.Persons.Roles;

namespace Data.FluentAPIs.Persons;

public class UserRoleFluent : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(p => new { p.UserId, p.RoleId });
    }
}