using GymManagementSystem.Domain.Common;

namespace GymManagementSystem.Domain.Entities;

public class Package : BaseModel
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;
    public int? MaxMembers { get; set; }
}
