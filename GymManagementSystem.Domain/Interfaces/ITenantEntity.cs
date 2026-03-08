namespace GymManagementSystem.Domain.Interfaces;

/// <summary>
/// Interface for multi-tenant entities.
/// Every entity implementing this belongs to a specific Gym (tenant).
/// </summary>
public interface ITenantEntity
{
    Guid GymId { get; set; }
}
