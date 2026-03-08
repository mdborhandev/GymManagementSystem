using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Payment : BaseEntity, ITenantEntity
{
    public Guid GymId { get; set; }
    public Guid MemberId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}
