using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course.CourseSeason;

namespace Services.Contracts.Interfaces;

public interface ICourseSeasonServices
{
    Task<IEnumerable<ShowSeasonsForCourseDto>> GetSeasonsForCourseAsync(int courseId ,CancellationToken cancellationToken = default, bool? isDeleted = null);
    Task<bool> IsCourseSeasonExistsAsync(string seasonTitle, int courseId, CancellationToken cancellationToken = default);
    Task<bool> IsCourseSeasonExistsAsync(int seasonId, CancellationToken cancellationToken = default);
    Task<EditSeasonDto> GetCourseSeasonForEditAsync(int seasonId, CancellationToken cancellationToken = default);
    Task<CourseSeason> GetCourseSeasonByIdAsync(int seasonId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<int> GetCourseIdBySeasonIdAsync(int seasonId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SelectListItem>> GetSeasonListByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);


    Task AddCourseSeasonAsync(CourseSeason courseSeason, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseSeasonAsync(CourseSeason courseSeason, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
