using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Member : BaseEntity, ITenantEntity
{
    public Guid GymId { get; set; }
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateTime JoinDate { get; set; }
    public Guid PackageId { get; set; }
}
