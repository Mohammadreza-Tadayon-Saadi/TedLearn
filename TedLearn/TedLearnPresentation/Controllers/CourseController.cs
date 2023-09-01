using Core.Utilities;
using Data.Entities.Persons.Discounts;
using Data.Entities.Products.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.Home.Order;
using SharpCompress.Archives;

namespace TedLearnPresentation.Controllers;

[Route("/Home/Courses")]
public class CourseController : Controller
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    private readonly ICourseEpisodeServices _courseEpisodeServices;
    private readonly ICourseGroupServices _courseGroupServices;
    private readonly ICourseCommentServices _courseCommentServices;
    private readonly ITransactionDbContextServices _transactions;
    private readonly IUserServices _userServices;
    private readonly IOrderServices _orderServices;
    private readonly IDiscountServices _discountServices;
    public CourseController(ICourseServices courseServices, IUserServices userServices, ICourseGroupServices courseGroupServices, ICourseEpisodeServices courseEpisodeServices, ICourseCommentServices courseCommentServices, ITransactionDbContextServices transactions, IOrderServices orderServices, IDiscountServices discountServices)
    {
        _courseServices = courseServices;
        _userServices = userServices;
        _courseGroupServices = courseGroupServices;
        _courseEpisodeServices = courseEpisodeServices;
        _courseCommentServices = courseCommentServices;
        _transactions = transactions;
        _orderServices = orderServices;
        _discountServices = discountServices;
    }

    #endregion

    [Route("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken, string category = "all",
        int pageId = 1, string filterByCourseTitle = "", string priceType = "", string orderByType = "", int take = 36)
    {
        ViewBag.CurrentCategory = category;
        ViewBag.OrderByType = orderByType;
        ViewBag.PriceType = priceType;
        ViewBag.CourseTitle = filterByCourseTitle;

        var model = await _courseServices.GetFilteredCoursesAsync(category, filterByCourseTitle, priceType,
                orderByType, take, pageId, cancellationToken);

        return View(model);
    }


    [Route("ShowCourse/{courseTitle}")]
    public async Task<IActionResult> ShowCourse(string courseTitle, CancellationToken cancellationToken)
    {
        var course = await _courseServices.GetCourseForShowAsync(courseTitle, cancellationToken);
        if (course == null) return NotFound();

        #region Check ViewData

        if (TempData.ContainsKey("resultOfAddOrder"))
        {
            var addResult = TempData["resultOfAddOrder"].ToString();

            if (addResult.StartsWith("Sql"))
                ViewBag.SqlException = true;
            else if (addResult.StartsWith("App"))
                ViewBag.AppOrTimeOutExeption = true;

            TempData.Remove("resultOfAddOrder");
        }

        if (TempData.ContainsKey("IsUserInCourse"))
        {
            ViewBag.IsUserInCourse = TempData["IsUserInCourse"];
            TempData.Remove("IsUserInCourse");
        }

        #endregion Check ViewData

        return View(course);
    }


    #region DownloadEpisodeFile

    [Route("/DownloadEpisode/{episodeTitle}/{seasonId:int}")]
    public async Task<IActionResult> DownloadEpisode(string episodeTitle, int seasonId, CancellationToken cancellationToken)
    {
        var episode = await _courseEpisodeServices.GetEpisodeForDownloadAsync(episodeTitle, seasonId, cancellationToken);
        if (episode == null) return Forbid();

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/course-episodes-file", episode.EpisodeFile);

        if (episode.IsFree)
        {
            byte[] file = System.IO.File.ReadAllBytes(filePath);
            return File(file, "application/force-download", episode.EpisodeFile);
        }
        else if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.GetUserId<int>();
            if (await _courseServices.IsUserInCourseAsync(userId, episode.CourseId) || userId == episode.TeacherId)
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/force-download", episode.EpisodeFile);
            }
        }

        return Forbid();
    }

    #endregion


    #region ShowOnlineEpisode

    [Route("ShowCourse/OnlineEpisode/{courseTitle}/{episodeId}")]
    [HttpPost]
    public async Task<IActionResult> OnlineEpisode(string courseTitle, int episodeId, CancellationToken cancellationToken)
    {
        if (!await _courseServices.IsCourseExistAsync(courseTitle, cancellationToken))
            return Json("404");

        var episode = await _courseEpisodeServices.GetEpisodeForShowOnlineAsync(episodeId, cancellationToken);
        if (episode == null) return Json("404");

        var directory = Directory.GetCurrentDirectory();
        var sourceVideoTag = "";

        var mp4File = episode.EpisodeFile.Replace(".rar", ".mp4");
        var filePathForShow = "";
        var targetPath = "";
        if (episode.IsFree)
        {
            targetPath = Path.Combine(directory, "wwwroot/Courses/course-episodes-video/free", mp4File);
            filePathForShow = $"/Courses/course-episodes-video/free/{mp4File}";
            if (!System.IO.File.Exists(targetPath))
            {
                var rarPath = Path.Combine(directory, "wwwroot/Courses/course-episodes-file", episode.EpisodeFile);

                using (var archive = ArchiveFactory.Open(rarPath))
                {
                    var entries = archive.Entries.OrderBy(e => e.Key.Length);
                    foreach (var en in entries)
                        if (Path.GetExtension(en.Key) == ".mp4" && Path.GetFileName(en.Key) == mp4File)
                            en.WriteTo(new FileStream(targetPath, FileMode.Create));
                }
            }

            sourceVideoTag = $"<video controls class='w-full h-full rounded-t-lg sm:rounded-lg' poster='/Courses/course-image/{episode.CourseImage}'><source src='{filePathForShow}' type='video/mp4'></video>";
            return Json(sourceVideoTag);
        }
        else
        {
            if (!User.Identity.IsAuthenticated)
                return Json($"/Account/Login?ReturnUrl=/Home/ShowCourse/{courseTitle}");

            var userId = User.Identity.GetUserId<int>();
            var isUserInCourse = await _courseServices.IsUserInCourseAsync(userId, episode.CourseId);
            if (!isUserInCourse && userId != episode.TeacherId) return Json("403");
            else
            {
                targetPath = Path.Combine(directory, "wwwroot/Courses/course-episodes-video/purchacable", mp4File);
                filePathForShow = $"/Courses/course-episodes-video/purchacable/{mp4File}";
                if (!System.IO.File.Exists(targetPath))
                {
                    var rarPath = Path.Combine(directory, "wwwroot/Courses/course-episodes-file", episode.EpisodeFile);

                    using (var archive = ArchiveFactory.Open(rarPath))
                    {
                        var entries = archive.Entries.OrderBy(e => e.Key.Length);
                        foreach (var en in entries)
                            if (Path.GetExtension(en.Key) == ".mp4" && Path.GetFileName(en.Key) == mp4File)
                                en.WriteTo(new FileStream(targetPath, FileMode.Create));
                    }
                }

                sourceVideoTag = $"<video controls class='w-full h-full rounded-t-lg sm:rounded-lg' poster='/Courses/course-image/{episode.CourseImage}'><source src='{filePathForShow}' type='video/mp4'></video>";
                return Json(sourceVideoTag);
            }
        }
    }

    #endregion


    #region Comment

    [Route("ShowComment/{courseId:int}")]
    public async Task<IActionResult> ShowComment(int courseId, CancellationToken cancellationToken, int pageId = 1)
    {
        if (!await _courseServices.IsCourseExistAsync(courseId, cancellationToken)) return PartialView("_Error404");

        var model = await _courseCommentServices.GetCommentForCourseAsync(courseId, pageId, cancellationToken);

        return PartialView("ShowComment", model);
    }

    [Route("AddComment/{courseId:int}")]
    [HttpPost]
    public async Task<IActionResult> AddComment(int courseId, string comment, int? replyId, int pageId, CancellationToken cancellationToken)
    {
        if (String.IsNullOrEmpty(comment)) return Redirect($"/Home/Courses/ShowComment/{courseId}?pageId={pageId}");

        if (User.Identity.IsAuthenticated)
        {
            if (!await _courseServices.IsCourseExistAsync(courseId, cancellationToken))
                return Redirect($"/Home/Courses/ShowComment/{courseId}?pageId={pageId}");

            var userId = User.Identity.GetUserId<int>();

            var courseComment = new CourseComment()
            {
                Comment = comment.SanitizeText(),
                CourseId = courseId,
                CreateDate = DateTime.Now,
                UserId = userId,
                ParentId = replyId
            };

            try
            {
                await _courseCommentServices.AddCommentAsync(courseComment, cancellationToken);
            }
            catch
            {
                TempData["AddCommentError"] = true;
            }
        }

        return Redirect($"/Home/Courses/ShowComment/{courseId}?pageId={pageId}");
    }

    [Route("/Home/Courses/DeleteComment/{commentId:int}/{courseId:int}/{pageId:int}")]
    [HttpPost]
    public async Task<IActionResult> DeleteComment(int commentId, int courseId, int pageId, CancellationToken cancellationToken)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!await _courseServices.IsCourseExistAsync(courseId)) return NoContent();

            var comment = await _courseCommentServices.GetCommentByIdAsync(commentId);
            if (comment == null) return PartialView("_Error404");

            comment.IsDelete = true;

            try
            {
                await _transactions.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                TempData["DeleteCommentError"] = true;
            }
        }

        return Redirect($"/Home/Courses/ShowComment/{courseId}?pageId={pageId}");
    }

    #endregion


    #region BuyCourse

    [Authorize]
    [Route("BuyCourse/{courseTitle}")]
    public async Task<IActionResult> BuyCourse(string courseTitle, CancellationToken cancellationToken)
    {
        var courseId = await _courseServices.GetCourseIdByCourseTitleAsync(courseTitle, cancellationToken);
        if (courseId < 1) return NotFound();

        var userId = User.Identity.GetUserId<int>();

        if (await _courseServices.IsUserInCourseAsync(userId, courseId, cancellationToken))
        {
            TempData["IsUserInCourse"] = true;
            return Redirect($"/Home/ShowCourse/{courseTitle}");
        }

        #region Add Order

        try
        {
            var orderId = await _orderServices.AddOrderAsync(courseId, userId, cancellationToken);

            TempData["resultOfAddOrder"] = "Successed";
            return Redirect($"/Home/Courses/Basket/{orderId}");
        }
        catch (SqlException)
        {
            TempData["resultOfAddOrder"] = "SqlException";
            return Redirect($"/Home/ShowCourse/{courseTitle}");
        }
        catch (Exception)
        {
            TempData["resultOfAddOrder"] = "AppOrTimeOutExeption";
            return Redirect($"/Home/ShowCourse/{courseTitle}");
        }

        #endregion Add Order
    }

    #endregion


    #region Basket

    [Authorize]
    [Route("Basket/{orderId:int}")]
    public async Task<IActionResult> Basket(int orderId, CancellationToken cancellationToken)
    {
        if (!await _orderServices.IsOrderExistAsync(orderId)) return NotFound();

        #region Check ViewData

        if (TempData.ContainsKey("resultOfAddOrder"))
        {
            var addResult = TempData["resultOfAddOrder"].ToString();
            if (addResult.StartsWith("Suc"))
                ViewBag.AddOrderSuccessed = true;

            TempData.Remove("resultOfAddOrder");
        }

        if (TempData.ContainsKey("DiscountType"))
        {
            ViewBag.DiscountType = TempData["DiscountType"].ToString();
            TempData.Remove("DiscountType");
        }

        if (TempData.ContainsKey("DiscountResult"))
        {
            var discountResult = TempData["DiscountResult"].ToString();

            if (discountResult.StartsWith("Sql"))
                ViewBag.SqlException = true;
            else if (discountResult.StartsWith("Suc"))
                ViewBag.SuccessedAddDiscount = true;
            else if (discountResult.StartsWith("Con"))
                ViewBag.Concurrency = true;
            else
                ViewBag.AppOrTimeOutExeption = true;

            TempData.Remove("DiscountResult");
        }

        if (TempData.ContainsKey("resultOfRemoveOrder"))
        {
            var removeResult = TempData["resultOfRemoveOrder"].ToString();

            if (removeResult.StartsWith("Suc"))
                ViewBag.RemoveDetailSuccessed = true;
            else if (removeResult.StartsWith("Sql"))
                ViewBag.SqlException = true;
            else ViewBag.AppOrTimeOutExeption = true;

            TempData.Remove("resultOfRemoveOrder");
        }

        #endregion Check ViewData

        var model = await _orderServices.GetBasketForUserAsync(orderId, User.Identity.GetUserId<int>(), cancellationToken);

        return View(model);
    }

    [Authorize]
    [Route("/AddDiscountToOrder/{orderId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddDiscountToOrder(int orderId, string code, CancellationToken cancellationToken)
    {
        var order = await _orderServices.GetOrderByIdAsync(orderId);
        if (order == null) return NotFound();

        var userId = User.Identity.GetUserId<int>();

        var discount = await _discountServices.GetUDiscountByCodeAsync(code.Trim(), cancellationToken, getActive: true);
        var checkDiscount = await _discountServices.CheckDiscountForApplyToOrderAsync(discount, userId, cancellationToken);
        if (checkDiscount == DiscountUseType.Successed)
        {
            // Remove Old Discount For This Order If Exists
            if (order.Discount != 0)
            {
                var userDiscount = await _discountServices.GetUserDiscountByOrderIdAsync(orderId, cancellationToken);
                if (userDiscount != null)
                {
                    var discountId = userDiscount.DiscountId;
                    await _discountServices.RemoveUserDiscountAsync(userDiscount, cancellationToken, withSaveChanges: false);
                    var oldDiscount = await _discountServices.GetUDiscountByIdAsync(discountId, cancellationToken);
                    oldDiscount.UsableCount++;
                }
            }

            order.Discount = (decimal)discount.Percent / 100;
            discount.UsableCount -= 1;
            discount.UserDiscounts = new List<UserDiscount>
            {
                new UserDiscount
                {
                    UserId = userId,
                    OrderId = orderId,
                    Percent = discount.Percent,
                    UseDate = DateTime.Now,
                }
            };

            #region Save Transaction

            try
            {
                await _transactions.SaveChangesAsync(cancellationToken);
                TempData["DiscountResult"] = "Success";
            }
            catch (SqlException ex)
            {
                TempData["DiscountResult"] = "SqlException";
                return Redirect($"/Home/Courses/Basket/{orderId}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["DiscountResult"] = "Concurrency";
                return Redirect($"/Home/Courses/Basket/{orderId}");
            }
            catch (DbUpdateException ex)
            {
                TempData["DiscountResult"] = "SqlException";
                return Redirect($"/Home/Courses/Basket/{orderId}");
            }
            catch
            {
                TempData["DiscountResult"] = "AppOrTimeOutExeption";
                return Redirect($"/Home/Courses/Basket/{orderId}");
            }

            #endregion Save Transaction
        }

        if (checkDiscount != DiscountUseType.Successed)
            TempData["DiscountType"] = checkDiscount.ToString();

        return Redirect($"/Home/Courses/Basket/{orderId}");
    }

    [Authorize]
    [Route("Basket/RemoveOrderDetails/{detailId:int}")]
    public async Task<IActionResult> RemoveOrderDetails(int detailId, CancellationToken cancellationToken)
    {
        var detail = await _orderServices.GetOrderDetailByIdAsync(detailId, cancellationToken);
        if (detail == null) return NotFound();

        int orderId = detail.OrderId;

        #region Remove Detail

        try
        {
            await _orderServices.RemoveOrderDetailAsync(detail, cancellationToken);

            TempData["resultOfRemoveOrder"] = "Successed";

            return Redirect($"/Home/Courses/Basket/{orderId}");
        }
        catch (SqlException)
        {
            TempData["resultOfRemoveOrder"] = "SqlException";
            return Redirect($"/Home/Courses/Basket/{orderId}");
        }
        catch
        {
            TempData["resultOfRemoveOrder"] = "AppOrTimeOutExeption";
            return Redirect($"/Home/Courses/Basket/{orderId}");
        }

        #endregion Remove Detail
    }

    #endregion
}
