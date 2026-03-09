using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Payment : BaseModel, ITenantEntity
{
    public Guid GymId { get; set; }
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = default!;
    
    public Guid? SubscriptionId { get; set; }
    
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; set; }
    public string? TransactionReference { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    public string? Remarks { get; set; }
}
