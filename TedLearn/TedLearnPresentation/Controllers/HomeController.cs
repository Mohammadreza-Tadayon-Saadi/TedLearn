namespace TedLearnPresentation.Controllers;

public class HomeController : Controller
{
    #region ConstructorInjection

    private readonly string wRoot;
    public HomeController(IWebHostEnvironment env)
    {
        wRoot = env.WebRootPath;
    }

    #endregion


    [Route("/")]
    [Route("/Home")]
    public IActionResult Index()
    {
        if (TempData.ContainsKey("SuccesSign"))
        {
            ViewBag.SuccessSign = TempData["SuccesSign"];
            TempData.Remove("SuccesSign");
        }
        return View();
    }
}
