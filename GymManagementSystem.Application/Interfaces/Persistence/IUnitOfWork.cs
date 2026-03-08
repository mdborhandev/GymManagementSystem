using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Gym> Gyms { get; }
    IGenericRepository<Member> Members { get; }
    IGenericRepository<Package> Packages { get; }
    IGenericRepository<Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
