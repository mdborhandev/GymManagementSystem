using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IGymRepository Gyms { get; }
    IMemberRepository Members { get; }
    IPackageRepository Packages { get; }
    IGenericRepository<Payment, Guid> Payments { get; } // Simple
    IStaffRepository Staff { get; }
    IGenericRepository<Attendance, Guid> Attendances { get; }
    IGenericRepository<Subscription, Guid> Subscriptions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
