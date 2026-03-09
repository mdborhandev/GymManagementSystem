namespace GymManagementSystem.Application.Interfaces.Services;

public interface ICurrentTenantService
{
    Guid? TenantId { get; }
    string? CurrentUserName { get; }
}
