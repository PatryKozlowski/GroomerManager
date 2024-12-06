using GroomerManager.Domain.Interface;
using GroomerManager.Infrastructure.Auth;
using GroomerManager.Infrastructure.Persistence.Configurations;
using GroomerManager.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration.GetConnectionString("GroomerManagerDB")!);
        services.AddJwt(configuration);
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}
