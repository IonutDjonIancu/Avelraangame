using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers;

[Route("/")]
public class HomeController : Controller
{
    [HttpGet("/Home")]
    public IActionResult Index()
    {
        return Ok("All okay!");
    }
}
