using GymManagementSystem.Application.DTOs.Gym;
using GymManagementSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

public class GymController : Controller
{
    private readonly IGymService _gymService;

    public GymController(IGymService gymService)
    {
        _gymService = gymService;
    }

    public async Task<IActionResult> Index()
    {
        var gyms = await _gymService.GetAllAsync();
        return View(gyms);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGymDto dto)
    {
        if (ModelState.IsValid)
        {
            await _gymService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var gym = await _gymService.GetByIdAsync(id);
        if (gym == null) return NotFound();
        
        var updateDto = new UpdateGymDto
        {
            Id = gym.Id,
            Name = gym.Name,
            Address = gym.Address,
            Phone = gym.Phone,
            Email = gym.Email,
            Website = gym.Website
        };
        
        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateGymDto dto)
    {
        if (ModelState.IsValid)
        {
            await _gymService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _gymService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
