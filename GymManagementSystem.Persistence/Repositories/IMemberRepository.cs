using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Persistence.Repositories;

public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync(Guid gymId);
    Task<Member?> GetByIdAsync(Guid id, Guid gymId);
    Task<Member> CreateAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Guid id, Guid gymId);
}
