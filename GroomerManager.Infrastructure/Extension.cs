using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}