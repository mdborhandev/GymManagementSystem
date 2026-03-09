using GymManagementSystem.Domain.Common;

namespace GymManagementSystem.Domain.Entities;

public class Attendance : BaseModel
{
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = default!;
    
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
    public DateTime? CheckOutTime { get; set; }
    public string? Remarks { get; set; }
}
