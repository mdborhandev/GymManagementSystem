using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Package : BaseModel, ITenantEntity
{
    public Guid GymId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;
    public int? MaxMembers { get; set; }
}
