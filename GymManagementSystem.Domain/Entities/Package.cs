using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Package : BaseEntity, ITenantEntity
{
    public Guid GymId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}
