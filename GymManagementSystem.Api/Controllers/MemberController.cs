using GymManagementSystem.Application.DTOs.Member;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class MemberController : Controller
{
    private readonly IUnitOfWork _uow;

    public MemberController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public IActionResult Index() => View();

    #region AJAX Endpoints
    [HttpGet]
    public async Task<JsonResult> GetAll()
    {
        var members = await _uow.Members.GetMembersWithPackagesAsync();
        var responses = members.Select(m => new MemberResponse
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Email = m.Email,
            Phone = m.Phone,
            JoinedDate = m.JoinDate,
            Status = m.Status.ToString(),
            StatusEnumValue = (int)m.Status,
            PackageName = m.Package?.Name ?? "Unknown",
            PackageId = m.PackageId,
            GymId = m.GymId
        });
        return Json(responses);
    }

    [HttpGet]
    public async Task<JsonResult> GetById(Guid id)
    {
        var m = await _uow.Members.GetByIdAsync(id, default);
        if (m == null) return Json(null);
        
        var package = await _uow.Packages.GetByIdAsync(m.PackageId, default);
        
        return Json(new MemberResponse
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Email = m.Email,
            Phone = m.Phone,
            JoinedDate = m.JoinDate,
            Status = m.Status.ToString(),
            StatusEnumValue = (int)m.Status,
            PackageName = package?.Name ?? "Unknown",
            PackageId = m.PackageId,
            GymId = m.GymId
        });
    }

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] CreateMemberDto dto)
    {
        if (!ModelState.IsValid) 
        {
            var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, message = "Validation failed: " + errors });
        }

        try 
        {
            var member = new Member
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                // Fix for PostgreSQL: Ensure DateTime is UTC
                DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
                Gender = dto.Gender,
                Address = dto.Address,
                PackageId = dto.PackageId,
                Status = dto.Status,
                JoinDate = DateTime.UtcNow
            };
            
            await _uow.Members.AddAsync(member);
            await _uow.SaveChangesAsync();
            return Json(new { success = true, id = member.Id });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Database error: " + ex.Message + (ex.InnerException != null ? " -> " + ex.InnerException.Message : "") });
        }
    }

    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] UpdateMemberDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, message = "Validation failed: " + errors });
        }

        try 
        {
            var m = await _uow.Members.GetByIdAsync(dto.Id, default);
            if (m == null) return Json(new { success = false, message = "Member not found" });

            m.FirstName = dto.FirstName;
            m.LastName = dto.LastName;
            m.Email = dto.Email;
            m.Phone = dto.Phone;
            // Fix for PostgreSQL: Ensure DateTime is UTC
            m.DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc);
            m.Gender = dto.Gender;
            m.Address = dto.Address;
            m.PackageId = dto.PackageId;
            m.Status = dto.Status;
            
            _uow.Members.Edit(m);
            await _uow.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Database error: " + ex.Message + (ex.InnerException != null ? " -> " + ex.InnerException.Message : "") });
        }
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        try 
        {
            var member = await _uow.Members.GetByIdAsync(id, default);
            if (member != null)
            {
                _uow.Members.Remove(member);
                await _uow.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Delete error: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<JsonResult> GetPackageList()
    {
        var packages = await _uow.Packages.GetAllAsync(default);
        return Json(packages.Select(p => new { p.Id, p.Name }));
    }
    #endregion
}
