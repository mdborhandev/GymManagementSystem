using GymManagementSystem.Application.DTOs.Package;

namespace GymManagementSystem.Application.Interfaces.Services;

public interface IPackageService
{
    Task<IEnumerable<PackageResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PackageResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePackageDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdatePackageDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
