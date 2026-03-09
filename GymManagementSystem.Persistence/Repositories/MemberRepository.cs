using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Persistence.Repositories;

public class MemberRepository : GenericRepository<Member, Guid>, IMemberRepository
{
    public MemberRepository(GymManagementDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Member>> GetMembersWithPackagesAsync(CancellationToken token = default)
    {
        return await _dbContext.Members
            .Include(m => m.Package)
            .Where(m => !m.IsDelete)
            .ToListAsync(token);
    }
}
