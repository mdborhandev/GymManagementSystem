using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Gym, Guid> Gyms { get; }
    IGenericRepository<Member, Guid> Members { get; }
    IGenericRepository<Package, Guid> Packages { get; }
    IGenericRepository<Payment, Guid> Payments { get; }
    IGenericRepository<Staff, Guid> Staff { get; }
    IGenericRepository<Attendance, Guid> Attendances { get; }
    IGenericRepository<Subscription, Guid> Subscriptions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
