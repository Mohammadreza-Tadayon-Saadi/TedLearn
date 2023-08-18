using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.Contracts.Interfaces;

public interface ICourseGroupServices
{
    /// <summary>
    ///     true => means just groups, 
    ///     false => means all courseGroups
    /// </summary>
    /// <param name="justGroup"></param>
    Task<IEnumerable<ShowCourseGroupDto>> GetCourseGroupsAsync(CancellationToken cancellationToken = default, bool? isDeleted = null, bool justGroup = true);
    Task<bool> IsCourseGroupExistAsync(string title , CancellationToken cancellationToken = default);
    Task<bool> IsCourseGroupExistAsync(int groupId, CancellationToken cancellationToken = default);
    Task<EditGroupDto> GetGroupForEditAsync(int groupId, CancellationToken cancellationToken = default);
    Task<CourseGroup> GetCourseGroupByGroupIdAsync(int groupId , CancellationToken cancellation, bool withTracking = true);
    Task<IEnumerable<ShowCourseGroupDto>> GetAllSubGroupsForGroupAsync(int groupId, CancellationToken cancellationToken = default, bool? isDeleted = null);
    Task<IEnumerable<SelectListItem>> GetGroupListAsync(CancellationToken cancellationToken = default , int? parentId = null);
    Task<IEnumerable<SelectListItem>> GetSubGroupListForGroup(int groupId, CancellationToken cancellationToken = default);

    Task AddCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
