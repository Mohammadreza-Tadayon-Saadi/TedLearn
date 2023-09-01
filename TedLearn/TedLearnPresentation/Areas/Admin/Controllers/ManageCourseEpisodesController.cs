using Core.Utilities;
using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.Course.CourseEpisode;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageCourseEpisodes")]
public class ManageCourseEpisodesController : Controller
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    private readonly ICourseSeasonServices _courseSeasonServices;
    private readonly ICourseEpisodeServices _courseEpisodeServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageCourseEpisodesController(ICourseServices courseServices, ICourseSeasonServices courseSeasonServices, 
        ICourseEpisodeServices courseEpisodeServices , ITransactionDbContextServices transaction)
    {
        _courseServices = courseServices;
        _courseSeasonServices = courseSeasonServices;
        _courseEpisodeServices = courseEpisodeServices;
        _transactions = transaction;
    }

    #endregion

    [Route("{seasonId:int}")]
    public async Task<IActionResult> Index(int seasonId, CancellationToken cancellationToken)
    {
        if (!await _courseSeasonServices.IsCourseSeasonExistsAsync(seasonId, cancellationToken)) return NotFound();

        ViewBag.SeasonId = seasonId;

        return View();
    }

    #region Ajax/Views

    [Route("GetEpisodes/GetAllEpisodes/{seasonId:int}")]
    [Route("GetEpisodes/GetDeletedEpisodes/{seasonId:int}")]
    public async Task<IActionResult> GetEpisodes(int seasonId, CancellationToken cancellationToken)
    {
        if (!await _courseSeasonServices.IsCourseSeasonExistsAsync(seasonId, cancellationToken))
            return PartialView("_Error404");

        IEnumerable<ShowEpisodesForSeasonDto> episodes;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllEpisodes"))
            episodes = await _courseEpisodeServices.GetEpisodesForSeasonAsync(seasonId, cancellationToken, isDeleted: false);
        else
            episodes = await _courseEpisodeServices.GetEpisodesForSeasonAsync(seasonId, cancellationToken, isDeleted: true);

        return PartialView("GetEpisodeAjax", episodes);
    }

    #endregion


    #region AddEpisodes

    [Route("AddEpisodes/{seasonId:int}")]
    public async Task<IActionResult> AddEpisodes(int seasonId, CancellationToken cancellationToken)
    {
        var courseId = await _courseSeasonServices.GetCourseIdBySeasonIdAsync(seasonId , cancellationToken);
        if (courseId == 0) return NotFound();

        var model = new AddEpisodeDto()
        {
            SeasonId = seasonId,
            CourseId = courseId,
            SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(courseId , cancellationToken)
        };

        return View(model);
    }

    [Route("AddEpisodes")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(2147483647)]
    [HttpPost]
    public async Task<IActionResult> AddEpisodes(AddEpisodeDto model, CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid)
        {
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId, cancellationToken);
            return View(model);
        }

        if (await _courseEpisodeServices.IsEpisodeExistAsync(model.Title.Trim(), model.SeasonId))
        {
            ModelState.AddModelError(nameof(model.Title), "عنوان وارد شده معتبر نمیباشد.لطفا عنوان دیگری انتخاب کنید.");
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId, cancellationToken);
            return View(model);
        }

        if (await _courseEpisodeServices.IsEpisodeFileExistAsync(model.EpisodeFile.FileName))
        {
            ModelState.AddModelError(nameof(model.EpisodeFile), "نام فایل مربوطه معتبر نمیباشد.لطفا نام دیگری انتخاب کنید.");
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId, cancellationToken);
            return View(model);
        }

        #endregion

        var directoryOfEpisode = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot/Courses/course-episodes-file");
        var episodeFile = await FileHelper.CreateFileAsync(model.EpisodeFile, directoryOfEpisode, true);
        var episode = new CourseEpisode()
        {
            SeasonId = model.SeasonId,
            EpisodeTitle = model.Title,
            EpisodeFile = episodeFile,
            EpisodeTime = model.Time,
            IsFree = (await _courseServices.IsCourseFreeAsync(model.CourseId , cancellationToken) ? true : (model.IsFree)),
            CreateDate = DateTime.Now
        };

        try
        {
            await _courseEpisodeServices.AddEpisodeAsync(episode , cancellationToken);
        }
        catch
        {
            FileHelper.DeleteFile(episodeFile, directoryOfEpisode);
            throw;
        }

        return RedirectToAction("Index", new { seasonId = episode.SeasonId });
    }

    #endregion AddEpisodes


    #region EditEpisodes

    [Route("EditEpisodes/{episodeId:int}")]
    public async Task<IActionResult> EditEpisodes(int episodeId , CancellationToken cancellationToken)
    {
        var episode = await _courseEpisodeServices.GetEpisodeForEditAsync(episodeId);
        if (episode == null) return NotFound();

        episode.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(episode.CourseId , cancellationToken);

        if (TempData.ContainsKey("ConcurrencyInEditEpisode"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditEpisode"];
            TempData.Remove("ConcurrencyInEditEpisode");
        }

        return View(episode);
    }

    [Route("EditEpisodes")]
    [HttpPost]
    [RequestSizeLimit(2147483647)]
    public async Task<IActionResult> EditEpisodes(EditEpisodeDto model, string preEpisodeTitle, CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid)
        {
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId, cancellationToken);
            return View(model);
        }

        if (preEpisodeTitle != model.Title && await _courseEpisodeServices.IsEpisodeExistAsync(model.Title.Trim(), model.SeasonId , cancellationToken))
        {
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId , cancellationToken);
            ModelState.AddModelError(nameof(model.Title), "عنوان وارد شده معتبر نمیباشد.لطفا عنوان دیگری انتخاب کنید.");
            return View(model);
        }

        if (model.File != null && await _courseEpisodeServices.IsEpisodeFileExistAsync(model.File.FileName , cancellationToken))
        {
            model.SeasonList = await _courseSeasonServices.GetSeasonListByCourseIdAsync(model.CourseId, cancellationToken);
            ModelState.AddModelError(nameof(model.File), "نام فایل مربوطه معتبر نمیباشد.لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        #endregion

        var episode = await _courseEpisodeServices.GetEpisodeByIdAsync(model.EpisodeId , cancellationToken);
        if (episode == null) return NotFound();

        // Concurrency Check
        if (Convert.ToBase64String(episode.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditEpisode"] = true;
            return Redirect($"/Admin/ManageCourseEpisodes/EditEpisodes/{model.EpisodeId}");
        }

        #region UpdateEpisode

        var episodeFile = "";
        var directoryOfEpisode = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/course-episodes-file");

        if (model.File != null)
        {
            episodeFile = await FileHelper.CreateFileAsync(model.File, directoryOfEpisode, true);
            episode.EpisodeFile = episodeFile;
        }

        episode.SeasonId = model.SeasonId;
        episode.EpisodeTime = model.Time;
        episode.EpisodeTitle = model.Title;
        episode.IsFree = (await _courseServices.IsCourseFreeAsync(model.CourseId , cancellationToken) ? true : (model.IsFree));

        try
        {
            await _transactions.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            FileHelper.DeleteFile(episodeFile, directoryOfEpisode);
            throw;
        }

        #endregion

        // Delete PreFile
        if (model.File != null)
            FileHelper.DeleteFile(model.PreEpisodeFileName, directoryOfEpisode);

        return RedirectToAction(nameof(Index), new { seasonId = episode.SeasonId });
    }

    #endregion


    #region DeleteEpisodes

    [Route("DeleteEpisodes/{episodeId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteEpisodes(int episodeId , CancellationToken cancellationToken)
    {
        var episode = await _courseEpisodeServices.GetEpisodeByIdAsync(episodeId, cancellationToken, getActive:true);
        if (episode == null) return PartialView("_Error404");

        episode.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect($"/Admin/ManageCourseEpisodes/GetEpisodes/GetAllEpisodes/{episode.SeasonId}");
    }

    #endregion


    #region RestoreEpisodes

    [Route("RestoreEpisodes/{episodeId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreEpisodes(int episodeId, CancellationToken cancellationToken)
    {
        var episode = await _courseEpisodeServices.GetEpisodeByIdAsync(episodeId, cancellationToken, getActive: false);
        if (episode == null) return PartialView("_Error404");

        episode.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect($"/Admin/ManageCourseEpisodes/GetEpisodes/GetDeletedEpisodes/{episode.SeasonId}");
    }

    #endregion

}
