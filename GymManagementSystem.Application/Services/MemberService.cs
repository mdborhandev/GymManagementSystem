using GymManagementSystem.Application.DTOs.Member;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Application.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MemberResponse>> GetAllMembersAsync(CancellationToken cancellationToken = default)
    {
        // Ideally use Include, but for now we fetch basic props
        var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);
        
        // Simple optimization: Fetch packages if needed, or rely on client/view
        // For simplicity in this demo, we might not have Package Name populated efficiently without Includes
        // Let's assume Package is loaded or we fetch separately
        
        var responses = new List<MemberResponse>();
        foreach(var m in members)
        {
            var package = await _unitOfWork.Packages.GetByIdAsync(m.PackageId, cancellationToken);
            responses.Add(new MemberResponse
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                JoinedDate = m.JoinDate,
                Status = m.Status.ToString(),
                PackageName = package?.Name ?? "Unknown",
                GymId = m.GymId
            });
        }
        return responses;
    }

    public async Task<MemberResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var m = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
        if (m == null) return null;
        
        var package = await _unitOfWork.Packages.GetByIdAsync(m.PackageId, cancellationToken);
        
        return new MemberResponse
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            Phone = m.Phone,
            JoinedDate = m.JoinDate,
            Status = m.Status.ToString(),
            PackageName = package?.Name ?? "Unknown",
            GymId = m.GymId
        };
    }

    public async Task<Guid> CreateAsync(CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var member = new Member
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            PackageId = dto.PackageId,
            Status = dto.Status,
            JoinDate = DateTime.UtcNow
        };
        
        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return member.Id;
    }

    public async Task UpdateAsync(UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(dto.Id, cancellationToken);
        if (member == null) return;
        
        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;
        member.Phone = dto.Phone;
        member.DateOfBirth = dto.DateOfBirth;
        member.Gender = dto.Gender;
        member.Address = dto.Address;
        member.PackageId = dto.PackageId;
        member.Status = dto.Status;
        
        _unitOfWork.Members.Edit(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
         var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
         if (member != null)
         {
             _unitOfWork.Members.Remove(member);
             await _unitOfWork.SaveChangesAsync(cancellationToken);
         }
    }
}
