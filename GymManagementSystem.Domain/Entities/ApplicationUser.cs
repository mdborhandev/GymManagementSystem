using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Guid? CompanyId { get; set; } // Link user to a specific company/gym
}
