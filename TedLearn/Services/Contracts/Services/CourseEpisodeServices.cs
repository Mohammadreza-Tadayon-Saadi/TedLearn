using Data.Context;
using Data.Entities.Persons.Users;
using Data.Entities.Products.Courses;
using Microsoft.EntityFrameworkCore;
using Services.Dto.AdminPanel.Course.CourseEpisode;
using Services.DTOs.AdminPanel.Course.CourseEpisode;

namespace Services.Contracts.Services;

public class CourseEpisodeServices : BaseServices<CourseEpisode> , ICourseEpisodeServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    public CourseEpisodeServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _transactions = transactions;
    }

    #endregion

    public async Task<IEnumerable<ShowEpisodesForSeasonDto>> GetEpisodesForSeasonAsync(int seasonId, CancellationToken cancellationToken = default, bool? isDeleted = null)
        => await ShowEpisodesForSeasonDto.ProjectTo(TableNoTracking.Where(cs => cs.SeasonId == seasonId && (isDeleted.HasValue) ? cs.IsDelete == isDeleted : true))
                            .ToListAsync(cancellationToken);

    public async Task<bool> IsEpisodeExistAsync(string episodeTitle, int seasonId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(ce => ce.EpisodeTitle == episodeTitle && ce.SeasonId == seasonId)
                            .AnyAsync(cancellationToken);

    public async Task<bool> IsEpisodeFileExistAsync(string fileName, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(ce => ce.EpisodeFile == fileName).AnyAsync(cancellationToken);

    public async Task<EditEpisodeDto> GetEpisodeForEditAsync(int episodeId, CancellationToken cancellationToken = default)
        => await EditEpisodeDto.ProjectTo(TableNoTracking
                                            .Include(ce => ce.CourseSeason)
                                            .Where(ce => ce.EpisodeId == episodeId))
                .SingleOrDefaultAsync(cancellationToken);

    public async Task<CourseEpisode> GetEpisodeByIdAsync(int episodeId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(u => u.EpisodeId == episodeId && ((getActive.HasValue) ? u.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(u => u.EpisodeId == episodeId && ((getActive.HasValue) ? u.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellationToken);
    }


    public async Task AddEpisodeAsync(CourseEpisode courseEpisode, CancellationToken cancellationToken = default, 
        bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(courseEpisode, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseEpisodeAsync(CourseEpisode courseEpisode, CancellationToken cancellationToken = default, 
        bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(courseEpisode);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
