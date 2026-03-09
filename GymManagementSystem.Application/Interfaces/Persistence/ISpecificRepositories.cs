using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IMemberRepository : IGenericRepository<Member, Guid>
{
    // Custom methods for Member if needed
    Task<IEnumerable<Member>> GetMembersWithPackagesAsync(CancellationToken token = default);
}

public interface IPackageRepository : IGenericRepository<Package, Guid> { }
public interface IGymRepository : IGenericRepository<Gym, Guid> { }
public interface IStaffRepository : IGenericRepository<Staff, Guid> { }
