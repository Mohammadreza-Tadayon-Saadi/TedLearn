using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Threading;

namespace TedLearnPresentation.Areas.UserPanel.Controllers;

[Authorize]
[Area("UserPanel")]
public class OrderController : Controller
{
    #region ConstructorInjection

    private readonly IOrderServices _orderServices;
    private readonly ICourseServices _courseServices;
    public OrderController(IOrderServices orderServices, ICourseServices courseServices)
    {
        _orderServices = orderServices;
        _courseServices = courseServices;
    }

    #endregion

    [Route("/UserPanel/MyOrders")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var orders = await _orderServices.GetOrdersForUserAsync(User.Identity.GetUserId<int>() , cancellationToken);

        if (TempData.ContainsKey("FinalyOrder"))
        {
            ViewBag.FinalyOrder = TempData["FinalyOrder"];
            TempData.Remove("FinalyOrder");
        }
        
        if (TempData.ContainsKey("BuyCourse"))
        {
            ViewBag.FinalyOrder = TempData["BuyCourse"];
            TempData.Remove("BuyCourse");
        }

        return View(orders);
    }


    [Route("/FinalyOrder/{orderId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> FinalyOrder(int orderId , CancellationToken cancellationToken)
    {
        if (await _orderServices.FinalyOrderAsync(User.Identity.GetUserId<int>(), orderId, cancellationToken))
        {
            TempData["FinalyOrder"] = true;
            return Redirect("/UserPanel/MyOrders");
        }

        return BadRequest();
    }


    [Route("/UserPanel/MyCourses")]
    public async Task<IActionResult> MyCourses(CancellationToken cancellationToken , int pageId = 1)
    {
        var model = await _courseServices.GetUserCoursesAsync(User.Identity.GetUserId<int>(), pageId: pageId , cancellationToken: cancellationToken);

        ViewBag.CurrentPage = pageId;

        return View(model);
    }
}
