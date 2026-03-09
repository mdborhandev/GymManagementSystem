using GymManagementSystem.Application.DTOs.Member;

namespace GymManagementSystem.Application.Interfaces.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberResponse>> GetAllMembersAsync(CancellationToken cancellationToken = default);
}
