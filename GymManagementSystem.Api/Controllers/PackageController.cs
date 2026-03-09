using GymManagementSystem.Application.DTOs.Package;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

public class PackageController : Controller
{
    private readonly IUnitOfWork _uow;

    public PackageController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<JsonResult> GetAll()
    {
        var packages = await _uow.Packages.GetAllAsync(default);
        var responses = packages.Select(p => new PackageResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            DurationInDays = p.DurationInDays,
            IsActive = p.IsActive,
            MaxMembers = p.MaxMembers
        });
        return Json(responses);
    }

    [HttpGet]
    public async Task<JsonResult> GetById(Guid id)
    {
        var p = await _uow.Packages.GetByIdAsync(id, default);
        if (p == null) return Json(null);
        return Json(new PackageResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            DurationInDays = p.DurationInDays,
            IsActive = p.IsActive,
            MaxMembers = p.MaxMembers
        });
    }

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] CreatePackageDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var package = new Package { Name = dto.Name, Description = dto.Description, Price = dto.Price, DurationInDays = dto.DurationInDays, IsActive = dto.IsActive, MaxMembers = dto.MaxMembers };
        await _uow.Packages.AddAsync(package);
        await _uow.SaveChangesAsync();
        return Json(new { success = true, id = package.Id });
    }

    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] UpdatePackageDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var p = await _uow.Packages.GetByIdAsync(dto.Id, default);
        if (p == null) return Json(new { success = false, message = "Package not found" });

        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.DurationInDays = dto.DurationInDays;
        p.IsActive = dto.IsActive;
        p.MaxMembers = dto.MaxMembers;

        _uow.Packages.Edit(p);
        await _uow.SaveChangesAsync();
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        var package = await _uow.Packages.GetByIdAsync(id, default);
        if (package != null)
        {
            _uow.Packages.Remove(package);
            await _uow.SaveChangesAsync();
        }
        return Json(new { success = true });
    }
}
