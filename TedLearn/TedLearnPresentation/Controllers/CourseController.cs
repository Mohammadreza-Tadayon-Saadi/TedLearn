namespace TedLearnPresentation.Controllers;

public class CourseController : Controller
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    private readonly ICourseGroupServices _courseGroupServices;
    private readonly IUserServices _userServices;
    public CourseController(ICourseServices courseServices, IUserServices userServices, ICourseGroupServices courseGroupServices)
    {
        _courseServices = courseServices;
        _userServices = userServices;
        _courseGroupServices = courseGroupServices;
    }

    #endregion


    [Route("/Home/Courses")]
    [Route("/Courses")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken ,string category = "all", 
        int pageId = 1, string filterByCourseTitle = "", string priceType = "", string orderByType = "" , int take = 36)
    {
        ViewBag.CurrentCategory = category;
        ViewBag.OrderByType = orderByType;
        ViewBag.PriceType = priceType;
        ViewBag.CourseTitle = filterByCourseTitle;

        var model = await _courseServices.GetFilteredCoursesAsync(category , filterByCourseTitle , priceType,
                orderByType , take , pageId , cancellationToken);

        return View(model);
    }
}
