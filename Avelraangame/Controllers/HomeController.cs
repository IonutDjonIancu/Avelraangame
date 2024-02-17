using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers;

[Route("/")]
public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return Ok("Mae govannen...");
    }
}
