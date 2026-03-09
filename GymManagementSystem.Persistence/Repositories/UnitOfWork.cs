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
        Gyms = new GenericRepository<Gym, Guid>(_context);
        Members = new GenericRepository<Member, Guid>(_context);
        Packages = new GenericRepository<Package, Guid>(_context);
        Payments = new GenericRepository<Payment, Guid>(_context);
        Staff = new GenericRepository<Staff, Guid>(_context);
        Attendances = new GenericRepository<Attendance, Guid>(_context);
        Subscriptions = new GenericRepository<Subscription, Guid>(_context);
    }

    public IGenericRepository<Gym, Guid> Gyms { get; }
    public IGenericRepository<Member, Guid> Members { get; }
    public IGenericRepository<Package, Guid> Packages { get; }
    public IGenericRepository<Payment, Guid> Payments { get; }
    public IGenericRepository<Staff, Guid> Staff { get; }
    public IGenericRepository<Attendance, Guid> Attendances { get; }
    public IGenericRepository<Subscription, Guid> Subscriptions { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
