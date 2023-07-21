using Data.Context;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.Contracts.Services;

public class CourseGroupServices : BaseServices<CourseGroup>, ICourseGroupServices
{
    #region ConstructorInjection

    public CourseGroupServices(TedLearnContext context) : base(context)
    {
    }

    #endregion ConstructorInjection

    public async Task<IEnumerable<ShowCourseGroupDto>> GetGroupsAsync(CancellationToken cancellationToken, bool? isDeleted = null)
        => await ShowCourseGroupDto.ProjectTo(TableNoTracking.Where(cg => cg.SubGroupId == 1 && ((isDeleted.HasValue) ? cg.IsDelete == isDeleted : true)))
                     .ToListAsync(cancellationToken);

    public async Task<bool> IsCourseGroupExistAsync(string title, CancellationToken cancellationToken)
        => await TableNoTracking.Where(cg => cg.Title == title).AnyAsync(cancellationToken);

    public async Task<bool> IsCourseGroupExistAsync(int groupId, CancellationToken cancellationToken)
        => await TableNoTracking.Where(cg => cg.GroupId == groupId).AnyAsync(cancellationToken);

    public async Task<EditGroupDto> GetGroupForEditAsync(int groupId, CancellationToken cancellationToken)
        => await EditGroupDto.ProjectTo(TableNoTracking.Where(cg => cg.GroupId == groupId))
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<CourseGroup> GetCourseGroupByGroupIdAsync(int groupId, CancellationToken cancellation, bool withTracking = true)
    {
        if (withTracking)
            return await Table.Where(cg => cg.GroupId == groupId).SingleOrDefaultAsync(cancellation);
        else
            return await TableNoTracking.Where(cg => cg.GroupId == groupId).SingleOrDefaultAsync(cancellation);
    }

    public async Task<IEnumerable<ShowCourseGroupDto>> GetAllSubGroupsForGroupAsync(int groupId, CancellationToken cancellationToken, bool? isDeleted = null)
        => await ShowCourseGroupDto.ProjectTo(TableNoTracking.Where(cg => cg.SubGroupId == groupId && ((isDeleted.HasValue) ? cg.IsDelete == isDeleted : true)))
                    .ToListAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetGroupListAsync(CancellationToken cancellationToken, int? parentId = null)
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



    public async Task AddCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(courseGroup, cancellationToken);

        if (withSaveChanges)
           await SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(courseGroup);

        if (withSaveChanges)
            await SaveChangesAsync(cancellationToken, configureAwait);
    }
}
