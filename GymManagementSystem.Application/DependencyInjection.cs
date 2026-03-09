using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        
        return services;
    }
}
