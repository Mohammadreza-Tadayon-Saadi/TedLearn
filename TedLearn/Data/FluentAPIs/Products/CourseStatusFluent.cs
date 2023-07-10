using Data.Entities.Products.Courses;

namespace Data.FluentAPIs.Products;

public class CourseStatusFluent : IEntityTypeConfiguration<CourseStatus>
{
    public void Configure(EntityTypeBuilder<CourseStatus> builder)
    {
        builder.HasData
            (
                new CourseStatus()
                {
                    StatusId = 1,
                    StatusTitle = "درحال برگزاری"
                },
                new CourseStatus()
                {
                    StatusId = 2,
                    StatusTitle = "پایان یافته"
                }
            );
    }
}
