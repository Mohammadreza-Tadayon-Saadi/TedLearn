using Data.Context;
using Data.Entities.Persons.Roles;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;
using Services.DTOs.AdminPanel.Course;

namespace Services.Contracts.Services;

public class CourseServices : BaseServices<Course>, ICourseServices
{
    #region ConstructorInjection

    private DbSet<CourseStatus> _courseStatus;
    private DbSet<UserRole> _userRole;
    private ICourseGroupServices _courseGroupServices;
    public CourseServices(TedLearnContext context , ICourseGroupServices courseGroupServices) : base(context)
    {
        _courseStatus = _context.Set<CourseStatus>();
        _userRole = _context.Set<UserRole>();
        _courseGroupServices = courseGroupServices;
    }

    #endregion

    public async Task<IEnumerable<SelectListItem>> GetStatusListAsync(CancellationToken cancellationToken)
        => await _courseStatus
                        .Select(cs => new SelectListItem()
                        {
                            Text = cs.StatusTitle,
                            Value = cs.StatusId.ToString()
                        }).ToListAsync(cancellationToken);

    public async Task<IEnumerable<SelectListItem>> GetTeacherListAsync(CancellationToken cancellationToken)
        => await _userRole
                        .Where(ur => ur.RoleId == 11003)
                        .Include(u => u.User)
                        .Where(u => !u.User.IsDelete)
                        .Select(u => new SelectListItem()
                        {
                            Text = u.User.UserName,
                            Value = u.User.UserId.ToString()
                        }).ToListAsync(cancellationToken);

    public async Task<IEnumerable<ShowCourseDto>> GetCoursesAsync(CancellationToken cancellationToken, bool? isDeleted = null)
        => await ShowCourseDto.ProjectTo(TableNoTracking.Include(c => c.User).Where(c => (isDeleted.HasValue) ? c.IsDelete == isDeleted : true))
                        .ToListAsync(cancellationToken);

    public async Task<ShowDetailsCourseDto> GetCourseDetailsAsync(int courseId, CancellationToken cancellationToken)
        => await ShowDetailsCourseDto.ProjectTo(TableNoTracking.Where(c => c.CourseId == courseId && !c.IsDelete)
                                                .Include(c => c.CourseToGroup)
                                                .Include(c => c.CourseToSubGroup)
                                                .Include(c => c.CourseStatus)
                                                .Include(c => c.User))
                                    .SingleOrDefaultAsync(cancellationToken);

    

    public async Task<EditCourseDto> GetCourseForEditAsync(int courseId, CancellationToken cancellationToken)
    {
        var dto = await EditCourseDto.ProjectTo(TableNoTracking.Where(c => c.CourseId == courseId))
                            .SingleOrDefaultAsync(cancellationToken);
        if (dto == null)
            return null;

        await GetSelectListsForCourseAsync(dto, cancellationToken);

        return dto;
    }

    public async Task GetSelectListsForCourseAsync(EditCourseDto dto, CancellationToken cancellationToken)
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

    public async Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken, bool withTracking = true, bool? getActive = null)
    {
        if(withTracking)
            return await Table.Where(c => c.CourseId == courseId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(c => c.CourseId == courseId && (getActive.HasValue ? c.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }



    public async Task AddCourseAsync(Course course, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(course, cancellationToken);

        if (withSaveChanges)
            await SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateCourseAsync(Course course, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(course);

        if (withSaveChanges)
            await SaveChangesAsync(cancellationToken, configureAwait);
    }
}
