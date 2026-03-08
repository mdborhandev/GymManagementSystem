using GymManagementSystem.Domain.Common;

namespace GymManagementSystem.Domain.Entities;

public class Gym : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
}
