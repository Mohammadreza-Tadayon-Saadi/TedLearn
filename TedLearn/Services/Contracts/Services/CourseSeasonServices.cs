using Data.Context;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.Course.CourseSeason;

namespace Services.Contracts.Services;

public class CourseSeasonServices : BaseServices<CourseSeason>, ICourseSeasonServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    private readonly ICourseServices _courseServices;
    public CourseSeasonServices(TedLearnContext context, ITransactionDbContextServices transactions, ICourseServices courseServices) : base(context)
    {
        _transactions = transactions;
        _courseServices = courseServices;
    }

    #endregion

    public async Task<IEnumerable<ShowSeasonsForCourseDto>> GetSeasonsForCourseAsync(int courseId,
        CancellationToken cancellationToken = default, bool? isDeleted = null)
        => await ShowSeasonsForCourseDto.ProjectTo(TableNoTracking
                                                    .Where(c => (isDeleted.HasValue) ? c.IsDelete == isDeleted : true))
                        .ToListAsync(cancellationToken);

    public async Task<bool> IsCourseSeasonExistsAsync(string seasonTitle, int courseId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cs => cs.SeasonTitle == seasonTitle && cs.CourseId == courseId)
                        .AnyAsync(cancellationToken);

    public async Task<bool> IsCourseSeasonExistsAsync(int seasonId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cs => cs.SeasonId == seasonId)
                        .AnyAsync(cancellationToken);

    public async Task<EditSeasonDto> GetCourseSeasonForEditAsync(int seasonId, CancellationToken cancellationToken = default)
    {
        var dto = await EditSeasonDto.ProjectTo(TableNoTracking.Where(cs => cs.SeasonId == seasonId)).SingleOrDefaultAsync(cancellationToken);
        dto.CourseList = await _courseServices.GetCourseSelectListAsync(cancellationToken);

        return dto;
    }

    public async Task<CourseSeason> GetCourseSeasonByIdAsync(int seasonId, CancellationToken cancellationToken = default,
        bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(c => c.SeasonId == seasonId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(c => c.SeasonId == seasonId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetCourseIdBySeasonIdAsync(int seasonId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cs => cs.SeasonId == seasonId)
                            .Select(cs => cs.CourseId)
                            .SingleOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetSeasonListByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cs => !cs.IsDelete && cs.CourseId == courseId)
                        .Select(cs => new SelectListItem()
                        {
                            Text = cs.SeasonTitle,
                            Value = cs.SeasonId.ToString()
                        }).ToListAsync(cancellationToken);



    public async Task AddCourseSeasonAsync(CourseSeason courseSeason, CancellationToken cancellationToken = default, 
        bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(courseSeason, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseSeasonAsync(CourseSeason courseSeason, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(courseSeason);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
