using GymManagementSystem.Application.Interfaces.Services;
using System.Security.Claims;

namespace GymManagementSystem.Api.Services;

public class CurrentTenantService : ICurrentTenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            // First check for GymId claim (from Auth Cookie)
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue("GymId");
            if (Guid.TryParse(claimValue, out var gymId))
                return gymId;

            // Fallback: Check header for backward compatibility
            var tenantIdHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-Gym-Id"].ToString();
            return Guid.TryParse(tenantIdHeader, out var headerGymId) ? headerGymId : null;
        }
    }
}
