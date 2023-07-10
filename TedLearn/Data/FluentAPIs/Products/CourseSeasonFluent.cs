using Data.Entities.Products.Courses;

namespace Data.FluentAPIs.Products;

public class CourseSeasonFluent : IEntityTypeConfiguration<CourseSeason>
{
    public void Configure(EntityTypeBuilder<CourseSeason> builder)
    {
        builder.HasIndex(p => p.SeasonTitle)
                   .IsUnique()
                   .HasDatabaseName("IX_CourseSeasons_SeasonTitle");
    }
}
