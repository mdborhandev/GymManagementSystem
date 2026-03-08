using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly GymManagementDbContext _context;

    public MemberRepository(GymManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllAsync(Guid gymId)
    {
        return await _context.Members
            .Where(m => m.GymId == gymId)
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(Guid id, Guid gymId)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id && m.GymId == gymId);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        member.UpdatedAt = DateTime.UtcNow;
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task DeleteAsync(Guid id, Guid gymId)
    {
        var member = await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id && m.GymId == gymId);

        if (member != null)
        {
            member.IsDeleted = true;
            member.UpdatedAt = DateTime.UtcNow;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
