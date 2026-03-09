using GymManagementSystem.Application.DTOs.Gym;

namespace GymManagementSystem.Application.Interfaces.Services;

public interface IGymService
{
    Task<IEnumerable<GymResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GymResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateGymDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateGymDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
