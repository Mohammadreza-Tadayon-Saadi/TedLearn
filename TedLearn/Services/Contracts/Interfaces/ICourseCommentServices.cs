using Data.Entities.Products.Comment;
using Services.DTOs.Home.CourseComment;

namespace Services.Contracts.Interfaces;

public interface ICourseCommentServices
{
    Task<CourseComment> GetCommentByIdAsync(int commentId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<ShowCourseCommentDto> GetCommentForCourseAsync(int courseId, int pageId = 1, CancellationToken cancellationToken = default);

    Task AddCommentAsync(CourseComment courseComment, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
