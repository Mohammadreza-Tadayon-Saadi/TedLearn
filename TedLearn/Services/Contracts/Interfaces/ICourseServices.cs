using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course;

namespace Services.Contracts.Interfaces;

public interface ICourseServices
{
    Task<IEnumerable<ShowCourseDto>> GetCoursesAsync(CancellationToken cancellationToken , bool? isDeleted = null);
    Task<ShowDetailsCourseDto> GetCourseDetailsAsync(int courseId, CancellationToken cancellationToken);
    Task<IEnumerable<SelectListItem>> GetTeacherListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<SelectListItem>> GetStatusListAsync(CancellationToken cancellationToken); // Get Status For Course
    Task<EditCourseDto> GetCourseForEditAsync(int courseId , CancellationToken cancellationToken);
    Task GetSelectListsForCourseAsync(EditCourseDto dto , CancellationToken cancellationToken);
    Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken, bool withTracking = true, bool? getActive = null);

    Task AddCourseAsync(Course course, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseAsync(Course course, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
}
