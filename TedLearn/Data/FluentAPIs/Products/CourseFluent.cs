using Data.Entities.Products.Courses;

namespace Data.FluentAPIs.Products;

public class CourseFluent : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasIndex(p => p.CourseTitle)
                   .IsUnique()
                   .HasDatabaseName("IX_Courses_CourseTitle");

        builder.HasCheckConstraint("CK_CoursePrice", "CoursePrice >= 0");
        builder.Property(p => p.Version).HasComment("This Column Is For Concurrency Check");
    }
}
