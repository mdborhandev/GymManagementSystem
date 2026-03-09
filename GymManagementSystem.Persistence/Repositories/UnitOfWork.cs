using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Persistence.Context;

namespace GymManagementSystem.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymManagementDbContext _context;

    public UnitOfWork(GymManagementDbContext context)
    {
        _context = context;
        Gyms = new GymRepository(_context);
        Members = new MemberRepository(_context);
        Packages = new PackageRepository(_context);
        Staff = new StaffRepository(_context);
        
        Payments = new GenericRepository<Payment, Guid>(_context);
        Attendances = new GenericRepository<Attendance, Guid>(_context);
        Subscriptions = new GenericRepository<Subscription, Guid>(_context);
    }

    public IGymRepository Gyms { get; }
    public IMemberRepository Members { get; }
    public IPackageRepository Packages { get; }
    public IStaffRepository Staff { get; }
    public IGenericRepository<Payment, Guid> Payments { get; }
    public IGenericRepository<Attendance, Guid> Attendances { get; }
    public IGenericRepository<Subscription, Guid> Subscriptions { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

// Simple specific repositories (can be in one file for now)
public class GymRepository : GenericRepository<Gym, Guid>, IGymRepository { public GymRepository(GymManagementDbContext dbContext) : base(dbContext) { } }
public class PackageRepository : GenericRepository<Package, Guid>, IPackageRepository { public PackageRepository(GymManagementDbContext dbContext) : base(dbContext) { } }
public class StaffRepository : GenericRepository<Staff, Guid>, IStaffRepository { public StaffRepository(GymManagementDbContext dbContext) : base(dbContext) { } }
