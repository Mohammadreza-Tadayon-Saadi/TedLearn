using Data.Entities.Products.Courses;

namespace Data.FluentAPIs.Products;

public class CourseGroupFluent : IEntityTypeConfiguration<CourseGroup>
{
    public void Configure(EntityTypeBuilder<CourseGroup> builder)
    {
        builder.HasIndex(p => p.Title)
                   .IsUnique()
                   .HasDatabaseName("IX_CourseGroups_Title");

        builder.Property(p => p.Version).HasComment("This Column Is For Concurrency Check");

        builder.HasData
            (
                new CourseGroup()
                {
                    GroupId = 1,
                    Title = "گروه های دوره ها"
                }
            );
    }
}
