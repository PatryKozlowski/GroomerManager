using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Infrastructure.Persistence.Configurations;
using GroomerManager.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DataTimeService>();
        services.AddDatabaseConfiguration(configuration.GetConnectionString("GroomerManagerStore")!);
        return services;
    }
}