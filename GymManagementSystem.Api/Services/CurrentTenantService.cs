using GymManagementSystem.Application.Interfaces.Services;

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
            var tenantIdHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-Gym-Id"].ToString();
            return Guid.TryParse(tenantIdHeader, out var tenantId) ? tenantId : null;
        }
    }
}
