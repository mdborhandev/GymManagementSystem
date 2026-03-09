using GymManagementSystem.Application.DTOs;
using GymManagementSystem.Application.Interfaces.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class HomeController : Controller
{
    private readonly IUnitOfWork _uow;

    public HomeController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalMembers = await _uow.Members.GetCountAsync(null),
            ActivePackages = await _uow.Packages.GetCountAsync(p => p.IsActive),
            TotalStaff = await _uow.Staff.GetCountAsync(null),
            // Placeholder for Revenue, as Payment entity logic is currently simple
            TotalRevenue = await _uow.Payments.GetCountAsync(null) * 50 // Dummy math for now
        };

        return View(model);
    }
}
