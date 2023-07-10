using Data.Entities.Persons.Roles;

namespace Data.FluentAPIs.Persons;

public class RoleFluent : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(p => p.RoleName)
                   .IsUnique()
                   .HasDatabaseName("IX_Roles_RoleName");

        builder.Property(p => p.Version).HasComment("This Column Is For Concurrency Check");

        #region Seed Data

        builder.HasData(
                        new Role()
                        {
                            RoleId = 11001,
                            RoleName = "کاربر عادی",
                            CanDeleteOrEdit = true,
                            CreateDate = DateTime.Now
                        },
                        new Role()
                        {
                            RoleId = 11002,
                            RoleName = "ادمین",
                            CanDeleteOrEdit = true,
                            CreateDate = DateTime.Now,
                        },
                        new Role()
                        {
                            RoleId = 11003,
                            RoleName = "استاد",
                            CanDeleteOrEdit = true,
                            CreateDate = DateTime.Now,
                        },
                        new Role()
                        {
                            RoleId = 11004,
                            RoleName = "مدیر سایت",
                            CanDeleteOrEdit = true,
                            CreateDate = DateTime.Now,
                        }
        );

        #endregion Seed Data
    }
}
