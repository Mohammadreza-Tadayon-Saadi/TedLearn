using Data.Context;
using Data.Entities.Products.Comment;
using Data.Entities.Products.Courses;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.Home.CourseComment;

namespace Services.Contracts.Services;

public class CourseCommentServices : BaseServices<CourseComment>, ICourseCommentServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    public CourseCommentServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _transactions = transactions;
    }

    #endregion ConstructorInjection


    public async Task<CourseComment> GetCommentByIdAsync(int commentId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(c => c.CommentId == commentId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(c => c.CommentId == commentId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ShowCourseCommentDto> GetCommentForCourseAsync(int courseId, int pageId = 1 , CancellationToken cancellationToken = default)
    {
        ShowCourseCommentDto model = new ShowCourseCommentDto();
        IQueryable<CourseComment> result = TableNoTracking
                                                .Include(cc => cc.User)
                                                .Include(cc => cc.Course)       
                                                .Where(cc => cc.CourseId == courseId && !cc.IsDelete);

        int take = 10;
        int skip = (pageId - 1) * take;

        int resCount = result.Count();

        int pageCount = resCount / take;
        if (resCount % take != 0) pageCount++;

        model.Comments = await ShowCourseCommentDetailsDto.ProjectTo(result
                                    .OrderByDescending(cc => cc.CreateDate)
                                    .Skip(skip)
                                    .Take(take)).ToListAsync(cancellationToken);
        
        model.Paginantion = new PaginantionDto
        {
            Currentpage = pageId,
            PageCount = pageCount,
            ItemsCount = resCount,
            ItemsPerPage = take,
        };

        return model;
    }



    public async Task AddCommentAsync(CourseComment courseComment, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(courseComment, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
