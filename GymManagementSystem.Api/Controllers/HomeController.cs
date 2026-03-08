using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
