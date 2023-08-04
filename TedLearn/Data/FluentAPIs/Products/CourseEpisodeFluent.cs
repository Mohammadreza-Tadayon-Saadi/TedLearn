using Data.Entities.Products.Courses;

namespace Data.FluentAPIs.Products;

public class CourseEpisodeFluent : IEntityTypeConfiguration<CourseEpisode>
{
    public void Configure(EntityTypeBuilder<CourseEpisode> builder)
    {
        builder.HasIndex(p => p.EpisodeTime).HasDatabaseName("IX_CourseEpisodes_EpisodeTime").IsUnique(false);
        builder.HasIndex(p => p.EpisodeTitle).HasDatabaseName("IX_CourseEpisodes_EpisodeTitle").IsUnique(false);
        builder.HasIndex(p => p.EpisodeFile).HasDatabaseName("IX_CourseEpisodes_EpisodeFile").IsUnique();
    }
}
