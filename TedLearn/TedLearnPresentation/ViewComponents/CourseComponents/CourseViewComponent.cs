using Data.Entities.Products.Courses;
using Services.DTOs.Home.Course;
using System.Linq.Expressions;

namespace TedLearnPresentation.ViewComponents.CourseComponents;

public class CourseViewComponent : ViewComponent
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    public CourseViewComponent(ICourseServices courseServices)
    {
        _courseServices = courseServices;
    }

    #endregion


    public async Task<IViewComponentResult> InvokeAsync(CourseViewComponentOptions options)
    {
        if (!options.CourseCardDtos.Any())
            options.CourseCardDtos = await _courseServices.GetCourseCardInfoAsync(options.OrderByExpression, take: options.Take);

        return await Task.FromResult((IViewComponentResult)View("/Views/Components/CourseComponent/CourseCard.cshtml",
                    options.CourseCardDtos));
    }
        
}

public class CourseViewComponentOptions
{
    public IEnumerable<ShowCourseCardDto> CourseCardDtos { get; set; }
    public Expression<Func<Course, object>> OrderByExpression { get; set; }
    public int Take { get; set; }

    public CourseViewComponentOptions(IEnumerable<ShowCourseCardDto> courseCardDtos = null, Expression<Func<Course, object>> orderByExpression = null , int take = 6)
    {
        CourseCardDtos = courseCardDtos ?? new List<ShowCourseCardDto>();
        OrderByExpression = orderByExpression ?? (c => c.CreateDate);
        Take = take;
    }
}
