namespace GymManagementSystem.Domain.Interfaces;

/// <summary>
/// Interface for multi-tenant entities.
/// </summary>
public interface ITenantEntity
{
    Guid CompanyId { get; set; }
}
