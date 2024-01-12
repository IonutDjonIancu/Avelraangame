using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Ok("All okay!");
    }
}
