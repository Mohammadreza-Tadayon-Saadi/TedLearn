using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.Course;
using Services.DTOs.AdminPanel.Course.CourseSeason;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageCourseSeasons")]
public class ManageCourseSeasonsController : Controller
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    private readonly ICourseSeasonServices _courseSeasonServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageCourseSeasonsController(ICourseServices courseServices, ICourseSeasonServices courseSeasonServices, ITransactionDbContextServices transactions)
    {
        _courseServices = courseServices;
        _courseSeasonServices = courseSeasonServices;
        _transactions = transactions;
    }

    #endregion


    [Route("{courseId:int}")]
    public async Task<IActionResult> Index(int courseId, CancellationToken cancellationToken)
    {
        if (!await _courseServices.IsCourseExistAsync(courseId, cancellationToken)) return NotFound();

        ViewBag.CourseId = courseId;

        return View();
    }


    #region Ajax/Views

    [Route("GetSeasons/GetAllSeasons/{courseId:int}")]
    [Route("GetSeasons/GetDeletedSeasons/{courseId:int}")]
    public async Task<IActionResult> GetSeasons(int courseId, CancellationToken cancellationToken)
    {
        if (!await _courseServices.IsCourseExistAsync(courseId, cancellationToken)) return PartialView("_Error404");

        IEnumerable<ShowSeasonsForCourseDto> seasons;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllSeasons"))
            seasons = await _courseSeasonServices.GetSeasonsForCourseAsync(courseId, cancellationToken, isDeleted: false);
        else
            seasons = await _courseSeasonServices.GetSeasonsForCourseAsync(courseId, cancellationToken, isDeleted: true);
        
        return PartialView("GetSeasonAjax", seasons);
    }

    #endregion


    #region AddSeasons

    [Route("AddSeasons/{courseId:int}")]
    public async Task<IActionResult> AddSeasons(int courseId, CancellationToken cancellationToken)
    {
        if (!await _courseServices.IsCourseExistAsync(courseId, cancellationToken)) return NotFound();

        var model = new AddSeasonDto()
        {
            CourseId = courseId,
            CourseList = await _courseServices.GetCourseSelectListAsync(cancellationToken),
        };

        return View(model);
    }

    [Route("AddSeasons")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddSeasons(AddSeasonDto model, CancellationToken cancellationToken)
    {
        #region ValidationInput

        if (!ModelState.IsValid)
            //model.CourseList = await _courseServices.GetCourseSelectListAsync(cancellationToken);
            return View(model);

        if (await _courseSeasonServices.IsCourseSeasonExistsAsync(model.Title.Trim(), model.CourseId, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "عنوان وارد شده قبلا برای این فصل انتخاب شده است.لطفا عنوان دیگری وارد کنید.");
            //model.CourseList = await _courseServices.GetCourseSelectListAsync(cancellationToken);
            return View(model);
        }

        #endregion

        var season = new CourseSeason()
        {
            CourseId = model.CourseId,
            SeasonTitle = model.Title,
            CreateDate = DateTime.Now,
        };
        await _courseSeasonServices.AddCourseSeasonAsync(season, cancellationToken);

        return RedirectToAction(nameof(Index), new { courseId = season.CourseId });
    }

    #endregion


    #region EditSeasons

    [Route("EditSeasons/{seasonId:int}")]
    public async Task<IActionResult> EditSeasons(int seasonId, CancellationToken cancellationToken)
    {
        var season = await _courseSeasonServices.GetCourseSeasonForEditAsync(seasonId, cancellationToken);
        if (season == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditSeason"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditSeason"];
            TempData.Remove("ConcurrencyInEditSeason");
        }

        return View(season);
    }

    [Route("EditSeasons")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditSeasons(EditSeasonDto model, string preTitle, CancellationToken cancellationToken)
    {
        #region ValidationInput

        if (!ModelState.IsValid)
            return View(model);

        if (model.Title.Trim() != preTitle &&
            await _courseSeasonServices.IsCourseSeasonExistsAsync(model.Title.Trim(), model.CourseId, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "عنوان وارد شده قبلا بذای این فصل انتخاب شده است.لطفا عنوان دیگری وارد کنید.");
            return View(model);
        }

        #endregion

        var season = await _courseSeasonServices.GetCourseSeasonByIdAsync(model.SeasonId);
        if (season == null) return NotFound();

        // ConCurrencyCheck
        if (Convert.ToBase64String(season.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditSeason"] = true;
            return Redirect($"EditSeasons/{model.SeasonId}");
        }

        season.SeasonTitle = model.Title;
        season.CourseId = model.CourseId;

        await _transactions.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Index), new { courseId = season.CourseId });
    }

    #endregion


    #region DeleteSeasons

    [Route("DeleteSeasons/{seasonId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteSeasons(int seasonId, CancellationToken cancellationToken)
    {
        var season = await _courseSeasonServices.GetCourseSeasonByIdAsync(seasonId, cancellationToken);
        if (season == null) return PartialView("_Error404");

        season.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect($"/Admin/ManageCourseSeasons/GetSeasons/GetAllSeasons/{season.CourseId}");
    }

    #endregion


    #region RestoreSeasons

    [Route("RestoreSeasons/{seasonId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreSeasons(int seasonId, CancellationToken cancellationToken)
    {
        var season = await _courseSeasonServices.GetCourseSeasonByIdAsync(seasonId, cancellationToken);
        if (season == null) return PartialView("_Error404");

        season.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect($"/Admin/ManageCourseSeasons/GetSeasons/GetDeletedSeasons/{season.CourseId}");
    }

    #endregion
}
