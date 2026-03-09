using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IGymService, GymService>();
        services.AddScoped<IPackageService, PackageService>();
        // services.AddScoped<IStaffService, StaffService>(); // StaffService implementation missing for now
        
        return services;
    }
}
