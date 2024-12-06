using GroomerManager.Application.Interfaces;
using GroomerManager.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
