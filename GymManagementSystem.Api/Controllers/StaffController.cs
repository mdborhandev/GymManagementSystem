using GymManagementSystem.Application.DTOs.Staff;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

public class StaffController : Controller
{
    private readonly IUnitOfWork _uow;

    public StaffController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public IActionResult Index() => View();

    #region AJAX Endpoints
    [HttpGet]
    public async Task<JsonResult> GetAll()
    {
        var staff = await _uow.Staff.GetAllAsync(default);
        var responses = staff.Select(s => new StaffResponse
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Role = s.Role.ToString(),
            IsActive = s.IsActive
        });
        return Json(responses);
    }

    [HttpGet]
    public async Task<JsonResult> GetById(Guid id)
    {
        var s = await _uow.Staff.GetByIdAsync(id, default);
        if (s == null) return Json(null);
        return Json(new StaffResponse
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Role = s.Role.ToString(),
            IsActive = s.IsActive
        });
    }

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] CreateStaffDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var staff = new Staff { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Phone = dto.Phone, Role = dto.Role, Salary = dto.Salary, HireDate = dto.HireDate, IsActive = dto.IsActive };
        await _uow.Staff.AddAsync(staff);
        await _uow.SaveChangesAsync();
        return Json(new { success = true, id = staff.Id });
    }

    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] UpdateStaffDto dto)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid data" });
        var s = await _uow.Staff.GetByIdAsync(dto.Id, default);
        if (s == null) return Json(new { success = false, message = "Staff not found" });

        s.FirstName = dto.FirstName;
        s.LastName = dto.LastName;
        s.Email = dto.Email;
        s.Phone = dto.Phone;
        s.Role = dto.Role;
        s.Salary = dto.Salary;
        s.IsActive = dto.IsActive;

        _uow.Staff.Edit(s);
        await _uow.SaveChangesAsync();
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        var s = await _uow.Staff.GetByIdAsync(id, default);
        if (s != null)
        {
            _uow.Staff.Remove(s);
            await _uow.SaveChangesAsync();
        }
        return Json(new { success = true });
    }
    #endregion
}
