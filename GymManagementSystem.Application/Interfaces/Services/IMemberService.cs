using GymManagementSystem.Application.DTOs.Member;

namespace GymManagementSystem.Application.Interfaces.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberResponse>> GetAllMembersAsync(CancellationToken cancellationToken = default);
    Task<MemberResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateMemberDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateMemberDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
