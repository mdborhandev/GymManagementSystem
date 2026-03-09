using GymManagementSystem.Application.DTOs.Gym;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class GymController : Controller
{
    private readonly IUnitOfWork _uow;

    public GymController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public IActionResult Index() => View();

    #region AJAX Endpoints
    [HttpGet]
    public async Task<JsonResult> GetAll()
    {
        var gyms = await _uow.Gyms.GetAllAsync(default);
        var responses = gyms.Select(g => new GymResponse
        {
            Id = g.Id,
            Name = g.Name,
            Address = g.Address,
            Phone = g.Phone,
            Email = g.Email,
            Website = g.Website
        });
        return Json(responses);
    }

    [HttpGet]
    public async Task<JsonResult> GetById(Guid id)
    {
        var g = await _uow.Gyms.GetByIdAsync(id, default);
        if (g == null) return Json(null);
        return Json(new GymResponse
        {
            Id = g.Id,
            Name = g.Name,
            Address = g.Address,
            Phone = g.Phone,
            Email = g.Email,
            Website = g.Website
        });
    }

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] CreateGymDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var gym = new Gym { Name = dto.Name, Address = dto.Address, Phone = dto.Phone, Email = dto.Email, Website = dto.Website };
        await _uow.Gyms.AddAsync(gym);
        await _uow.SaveChangesAsync();
        return Json(new { success = true, id = gym.Id });
    }

    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] UpdateGymDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var gym = await _uow.Gyms.GetByIdAsync(dto.Id, default);
        if (gym == null) return Json(new { success = false, message = "Gym not found" });

        gym.Name = dto.Name;
        gym.Address = dto.Address;
        gym.Phone = dto.Phone;
        gym.Email = dto.Email;
        gym.Website = dto.Website;

        _uow.Gyms.Edit(gym);
        await _uow.SaveChangesAsync();
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        var gym = await _uow.Gyms.GetByIdAsync(id, default);
        if (gym != null)
        {
            _uow.Gyms.Remove(gym);
            await _uow.SaveChangesAsync();
        }
        return Json(new { success = true });
    }
    #endregion
}
