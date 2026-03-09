using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Interfaces;

namespace GymManagementSystem.Domain.Entities;

public class Member : BaseModel, ITenantEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? Email { get; set; }
    public string Phone { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
    public MemberStatus Status { get; set; } = MemberStatus.Active;
    
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    
    // Relationships
    public Guid PackageId { get; set; }
    public Package Package { get; set; } = default!;
}
