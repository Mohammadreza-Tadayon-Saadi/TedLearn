using Microsoft.AspNetCore.Authorization;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    [Route("/Admin")]
    public IActionResult Index() => View();
}
