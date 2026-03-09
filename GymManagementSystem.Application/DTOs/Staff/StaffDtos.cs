using GymManagementSystem.Domain.Enums;

namespace GymManagementSystem.Application.DTOs.Staff;

public class CreateStaffDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string Phone { get; set; } = default!;
    public StaffRole Role { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}

public class UpdateStaffDto : CreateStaffDto
{
    public Guid Id { get; set; }
}

public class StaffResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
    public string Role { get; set; } = default!;
    public bool IsActive { get; set; }
}
