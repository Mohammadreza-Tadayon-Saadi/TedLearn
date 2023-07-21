using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course.CourseGroup;
using System.Threading;

namespace Services.Contracts.Interfaces;

public interface ICourseGroupServices
{
    Task<IEnumerable<ShowCourseGroupDto>> GetGroupsAsync(CancellationToken cancellationToken, bool? isDeleted = null);
    Task<bool> IsCourseGroupExistAsync(string title , CancellationToken cancellationToken);
    Task<bool> IsCourseGroupExistAsync(int groupId, CancellationToken cancellationToken);
    Task<EditGroupDto> GetGroupForEditAsync(int groupId, CancellationToken cancellationToken);
    Task<CourseGroup> GetCourseGroupByGroupIdAsync(int groupId , CancellationToken cancellation, bool withTracking = true);
    Task<IEnumerable<ShowCourseGroupDto>> GetAllSubGroupsForGroupAsync(int groupId, CancellationToken cancellationToken, bool? isDeleted = null);
    Task<IEnumerable<SelectListItem>> GetGroupListAsync(CancellationToken cancellationToken , int? parentId = null);

    Task AddCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseGroupAsync(CourseGroup courseGroup, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
}
