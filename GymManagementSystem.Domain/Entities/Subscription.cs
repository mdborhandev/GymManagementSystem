using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Enums;

namespace GymManagementSystem.Domain.Entities;

public class Subscription : BaseModel
{
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = default!;
    
    public Guid PackageId { get; set; }
    public Package Package { get; set; } = default!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AmountPaid { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
}
