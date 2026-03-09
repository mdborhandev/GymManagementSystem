using GymManagementSystem.Domain.Common;
using GymManagementSystem.Domain.Enums;

namespace GymManagementSystem.Domain.Entities;

public class Staff : BaseModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? Email { get; set; }
    public string Phone { get; set; } = default!;
    public StaffRole Role { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public string? ProfilePictureUrl { get; set; }
}
