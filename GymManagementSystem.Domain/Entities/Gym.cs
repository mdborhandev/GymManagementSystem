using GymManagementSystem.Domain.Common;

namespace GymManagementSystem.Domain.Entities;

public class Gym : BaseModel
{
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
}
