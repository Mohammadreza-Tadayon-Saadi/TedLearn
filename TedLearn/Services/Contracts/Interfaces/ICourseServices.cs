using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course;
using Services.DTOs.Home.Course;
using System.Linq.Expressions;

namespace Services.Contracts.Interfaces;

public interface ICourseServices
{
    Task<IEnumerable<ShowCourseDto>> GetCoursesAsync(CancellationToken cancellationToken = default , bool? isDeleted = null);
    Task<ShowDetailsCourseDto> GetCourseDetailsAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SelectListItem>> GetTeacherListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SelectListItem>> GetStatusListAsync(CancellationToken cancellationToken = default); // Get Status For Course
    Task<EditCourseDto> GetCourseForEditAsync(int courseId , CancellationToken cancellationToken = default);
    Task GetSelectListsForCourseAsync(EditCourseDto dto , CancellationToken cancellationToken = default);
    Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<bool> IsCourseExistAsync(int courseId , CancellationToken cancellationToken = default);
    Task<IEnumerable<SelectListItem>> GetCourseSelectListAsync(CancellationToken cancellationToken = default);
    Task<bool> IsCourseFreeAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShowCourseCardDto>> GetCourseCardInfoAsync(Expression<Func<Course, object>> orderByExpression , int take = 6, CancellationToken cancellationToken = default);
    Task<FilteredCoursesDto> GetFilteredCoursesAsync(string category = "all", string filterByCourseTitle = "", 
        string priceType = "", string orderByType = "", int take = 36, int pageId = 1, CancellationToken cancellationToken = default);


    Task AddCourseAsync(Course course, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseAsync(Course course, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
