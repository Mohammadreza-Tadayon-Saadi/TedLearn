using Data.Context;
using Data.Entities.Persons.Roles;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.Course;
using Services.DTOs.Home.Course;
using System.Linq.Expressions;

namespace Services.Contracts.Services;

public class CourseServices : BaseServices<Course>, ICourseServices
{
    #region ConstructorInjection

    private readonly DbSet<CourseStatus> _courseStatus;
    private readonly DbSet<UserRole> _userRole;
    private readonly ICourseGroupServices _courseGroupServices;
    private readonly ITransactionDbContextServices _transactions;
    
    public CourseServices(TedLearnContext context , ICourseGroupServices courseGroupServices , ITransactionDbContextServices transactions) : base(context)
    {
        _courseStatus = _context.Set<CourseStatus>();
        _userRole = _context.Set<UserRole>();
        _courseGroupServices = courseGroupServices;
        _transactions = transactions;
    }

    #endregion

    public async Task<IEnumerable<SelectListItem>> GetStatusListAsync(CancellationToken cancellationToken = default)
        => await _courseStatus
                        .Select(cs => new SelectListItem()
                        {
                            Text = cs.StatusTitle,
                            Value = cs.StatusId.ToString()
                        }).ToListAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetTeacherListAsync(CancellationToken cancellationToken = default)
        => await _userRole
                        .Where(ur => ur.RoleId == 11003)
                        .Include(u => u.User)
                        .Where(u => !u.User.IsDelete)
                        .Select(u => new SelectListItem()
                        {
                            Text = u.User.UserName,
                            Value = u.User.UserId.ToString()
                        }).ToListAsync(cancellationToken);

    public async Task<IEnumerable<ShowCourseDto>> GetCoursesAsync(CancellationToken cancellationToken = default, bool? isDeleted = null)
        => await ShowCourseDto.ProjectTo(TableNoTracking.Include(c => c.User).Where(c => (isDeleted.HasValue) ? c.IsDelete == isDeleted : true))
                        .ToListAsync(cancellationToken);

    public async Task<ShowDetailsCourseDto> GetCourseDetailsAsync(int courseId, CancellationToken cancellationToken = default)
        => await ShowDetailsCourseDto.ProjectTo(TableNoTracking.Where(c => c.CourseId == courseId && !c.IsDelete)
                                                .Include(c => c.CourseToGroup)
                                                .Include(c => c.CourseToSubGroup)
                                                .Include(c => c.CourseStatus)
                                                .Include(c => c.User))
                                    .SingleOrDefaultAsync(cancellationToken);

    

    public async Task<EditCourseDto> GetCourseForEditAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var dto = await EditCourseDto.ProjectTo(TableNoTracking.Where(c => c.CourseId == courseId))
                            .SingleOrDefaultAsync(cancellationToken);
        if (dto == null)
            return null;

        await GetSelectListsForCourseAsync(dto, cancellationToken);

        return dto;
    }

    public async Task GetSelectListsForCourseAsync(EditCourseDto dto, CancellationToken cancellationToken = default)
    {
        dto.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken);
        dto.SubGroupList = await _courseGroupServices.GetSubGroupListForGroup(dto.GroupId, cancellationToken);
        dto.TeacherList = await GetTeacherListAsync(cancellationToken);
        dto.StatusList = await GetStatusListAsync(cancellationToken);
        dto.LevelList = new List<SelectListItem>
        {
            new SelectListItem() { Text = "مقدماتی", Value = "مقدماتی", Selected = ((dto.CourseLevel == "مقدماتی") ? true : false) },
            new SelectListItem() { Text = "متوسط", Value = "متوسط", Selected = ((dto.CourseLevel == "متوسط") ? true : false) },
            new SelectListItem() { Text = "پیشرفته", Value = "پیشرفته", Selected = ((dto.CourseLevel == "پیشرفته") ? true : false) },
            new SelectListItem() { Text = "مقدماتی تا پیشرفته", Value = "مقدماتی تا پیشرفته", Selected = ((dto.CourseLevel == "مقدماتی تا پیشرفته") ? true : false) }
        };
    }

    public async Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if(withTracking)
            return await Table.Where(c => c.CourseId == courseId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(c => c.CourseId == courseId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsCourseExistAsync(int courseId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(c => c.CourseId == courseId).AnyAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetCourseSelectListAsync(CancellationToken cancellationToken = default)
        =>  await TableNoTracking.Where(c => !c.IsDelete)
                                .Select(c => new SelectListItem()
                                {
                                    Text = c.CourseTitle,
                                    Value = c.CourseId.ToString()
                                }).ToListAsync(cancellationToken);

    public async Task<bool> IsCourseFreeAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var price = await TableNoTracking.Where(c => c.CourseId == courseId).Select(c => c.CoursePrice).SingleOrDefaultAsync(cancellationToken);
        return price == 0 ? true : false;
    }

    public async Task<IEnumerable<ShowCourseCardDto>> GetCourseCardInfoAsync(Expression<Func<Course, object>> orderByExpression, int take = 6, CancellationToken cancellationToken = default)
        => await ShowCourseCardDto.ProjectTo(TableNoTracking
                                            .Where(c => !c.IsDelete)
                                            .Include(c => c.User)
                                            .Include(c => c.UserCourses)
                                            .Include(c => c.CourseToGroup)
                                            .OrderByDescending(orderByExpression)
                                            .Take(take))
                    .ToListAsync(cancellationToken);

    public async Task<FilteredCoursesDto> GetFilteredCoursesAsync(string category = "all", string filterByCourseTitle = "",
        string priceType = "", string orderByType = "", int take = 36 , int pageId = 1, CancellationToken cancellationToken = default)
    {
        var model = new FilteredCoursesDto();
        int skip = (pageId - 1) * take;

        IQueryable<Course> result = TableNoTracking
                                        .Where(c => !c.IsDelete)
                                        .Include(c => c.CourseToGroup)
                                        .Include(c => c.UserCourses);


        #region Filtering

        if (category != "all")
        {
            result = result.Include(c => c.CourseToSubGroup)
                        .Where(c => c.CourseToGroup.Title == category || c.CourseToSubGroup.Title == category);
        }

        if (!String.IsNullOrEmpty(filterByCourseTitle))
            result = result.Where(c => c.CourseTitle.Contains(filterByCourseTitle) || c.CourseTags.Contains(filterByCourseTitle));

        result = priceType switch
        {
            "All" => result,
            "Purchasable" => result.Where(c => c.CoursePrice != 0),
            "Free" => result.Where(c => c.CoursePrice == 0),
            _ => result
        };

        result = orderByType switch
        {
            "CreateAndUpdateDate" => result.OrderByDescending(c => c.LastUpdateDate).ThenByDescending(c => c.CreateDate),
            "Title" => result.OrderBy(c => c.CourseTitle),
            "Price" => result.OrderBy(c => c.CoursePrice),
            _ => result.OrderByDescending(c => c.LastUpdateDate).ThenByDescending(c => c.CreateDate)
        };

        #endregion Filtering


        var countOfCourse = result.Count();

        var pageCount = countOfCourse / take;
        if (countOfCourse % take != 0) pageCount++;

        model.CourseCards = await ShowCourseCardDto.ProjectTo(result.Skip(skip).Take(take)).ToListAsync(cancellationToken);
        model.CourseGroups = await _courseGroupServices.GetCourseGroupsAsync(cancellationToken, isDeleted: false , justGroup: false);
        model.Paginantion = new PaginantionDto
        {
            ItemsCount = countOfCourse,
            PageCount = pageCount,
            Currentpage = pageId,
            ItemsPerPage = take
        };

        return model;
    }



    public async Task AddCourseAsync(Course course, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(course, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseAsync(Course course, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(course);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
