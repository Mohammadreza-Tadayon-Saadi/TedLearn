namespace TedLearnPresentation.Controllers;

public class ErrorController : Controller
{
    [Route("/ConcurrencyException")]
    public IActionResult ConcurrencyException() => View();

    [Route("/DbUpdateException")]
    public IActionResult DbUpdateException() => View();

    [Route("/SqlException")]
    public IActionResult SqlException() => View();

    [Route("/TimeoutException")]
    public IActionResult TimeoutException() => View();

    [Route("/InternalException")]
    public IActionResult InternalException() => View();

    [Route("/Error404")]
    public IActionResult Error404() => View();
}
