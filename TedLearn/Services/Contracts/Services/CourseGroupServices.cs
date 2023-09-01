using Data.Context;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.Course.CourseGroup;
using System.Threading;

namespace Services.Contracts.Services;

public class CourseGroupServices : BaseServices<CourseGroup>, ICourseGroupServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    public CourseGroupServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _transactions = transactions;
    }

    #endregion ConstructorInjection

    public async Task<IEnumerable<ShowCourseGroupDto>> GetCourseGroupsAsync(CancellationToken cancellationToken = default, bool? isDeleted = null, bool justGroup = true)
        => await ShowCourseGroupDto.ProjectTo(TableNoTracking.Where(cg => (justGroup ? cg.SubGroupId == 1 : true) && ((isDeleted.HasValue) ? cg.IsDelete == isDeleted : true)))
                     .ToListAsync(cancellationToken);

    public async Task<bool> IsCourseGroupExistAsync(string title, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cg => cg.Title == title).AnyAsync(cancellationToken);

    public async Task<bool> IsCourseGroupExistAsync(int groupId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cg => cg.GroupId == groupId).AnyAsync(cancellationToken);

    public async Task<EditGroupDto> GetGroupForEditAsync(int groupId, CancellationToken cancellationToken = default)
        => await EditGroupDto.ProjectTo(TableNoTracking.Where(cg => cg.GroupId == groupId))
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<CourseGroup> GetCourseGroupByGroupIdAsync(int groupId, CancellationToken cancellation, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(cg => cg.GroupId == groupId && (getActive.HasValue ? cg.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellation);
        else
            return await TableNoTracking.Where(cg => cg.GroupId == groupId && (getActive.HasValue ? cg.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellation);
    }

    public async Task<IEnumerable<ShowCourseGroupDto>> GetAllSubGroupsForGroupAsync(int groupId, CancellationToken cancellationToken = default, bool? isDeleted = null)
        => await ShowCourseGroupDto.ProjectTo(TableNoTracking.Where(cg => cg.SubGroupId == groupId && ((isDeleted.HasValue) ? cg.IsDelete == isDeleted : true)))
                    .ToListAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetGroupListAsync(CancellationToken cancellationToken = default, int? parentId = null)
    {
        if(parentId.HasValue)
            return await TableNoTracking.Where(cg => cg.SubGroupId == 1 && !cg.IsDelete)
                                .Select(g => new SelectListItem
                                {
                                    Text = g.Title,
                                    Value = g.GroupId.ToString(),
                                    Selected = ((parentId == g.GroupId) ? true : false),
                                }).ToListAsync(cancellationToken);
        else
            return await TableNoTracking.Where(cg => cg.SubGroupId == 1 && !cg.IsDelete)
                                .Select(g => new SelectListItem
                                {
                                    Text = g.Title,
                                    Value = g.GroupId.ToString(),
                                }).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SelectListItem>> GetSubGroupListForGroup(int groupId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(cg => cg.SubGroupId == groupId && !cg.IsDelete)
                        .Select(cg => new SelectListItem()
                        {
                            Text = cg.Title,
                            Value = cg.GroupId.ToString()
                        }).ToListAsync(cancellationToken);



    public async Task AddCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(courseGroup, cancellationToken);

        if (withSaveChanges)
           await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(courseGroup);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
