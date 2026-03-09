using GymManagementSystem.Application.DTOs.Member;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Application.Interfaces.Services;

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
        var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);
        
        return members.Select(m => new MemberResponse
        {
            Id = m.Id,
            Name = m.FullName,
            JoinedDate = m.JoinDate,
            GymId = m.GymId
        });
    }
}
