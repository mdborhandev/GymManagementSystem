using GymManagementSystem.Application.DTOs.Staff;

namespace GymManagementSystem.Application.Interfaces.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StaffResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateStaffDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateStaffDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
