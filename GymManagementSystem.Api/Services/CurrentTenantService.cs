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
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true) return null;

            if (user.IsInRole("SuperAdmin")) return null;

            var claimValue = user.FindFirstValue("CompanyId");
            if (Guid.TryParse(claimValue, out var gymId))
                return gymId;

            return null;
        }
    }

    public string? CurrentUserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
