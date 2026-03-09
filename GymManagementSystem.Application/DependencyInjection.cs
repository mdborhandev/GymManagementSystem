using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application layer now relies on Repositories directly in Controllers
        return services;
    }
}
