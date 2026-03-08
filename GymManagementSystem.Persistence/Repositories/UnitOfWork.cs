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
        Gyms = new GenericRepository<Gym>(_context);
        Members = new GenericRepository<Member>(_context);
        Packages = new GenericRepository<Package>(_context);
        Payments = new GenericRepository<Payment>(_context);
    }

    public IGenericRepository<Gym> Gyms { get; }
    public IGenericRepository<Member> Members { get; }
    public IGenericRepository<Package> Packages { get; }
    public IGenericRepository<Payment> Payments { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
