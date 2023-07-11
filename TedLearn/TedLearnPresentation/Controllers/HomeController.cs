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
    [Route("/Home/{SuccessSign?}")]
    public IActionResult Index(string? SuccessSign)
    {
        if (SuccessSign != null) ViewBag.SuccessSign = true;
        return View();
    }
}
