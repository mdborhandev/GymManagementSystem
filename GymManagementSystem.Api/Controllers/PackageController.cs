using GymManagementSystem.Application.DTOs.Package;
using GymManagementSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

public class PackageController : Controller
{
    private readonly IPackageService _packageService;

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    public async Task<IActionResult> Index()
    {
        var packages = await _packageService.GetAllAsync();
        return View(packages);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePackageDto dto)
    {
        if (ModelState.IsValid)
        {
            await _packageService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var package = await _packageService.GetByIdAsync(id);
        if (package == null) return NotFound();
        
        var updateDto = new UpdatePackageDto
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Price = package.Price,
            DurationInDays = package.DurationInDays,
            IsActive = package.IsActive,
            MaxMembers = package.MaxMembers
        };
        
        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdatePackageDto dto)
    {
        if (ModelState.IsValid)
        {
            await _packageService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _packageService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
