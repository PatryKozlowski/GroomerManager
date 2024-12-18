using GroomerManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure.Persistence.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
    {
        Action<IServiceProvider, DbContextOptionsBuilder> sqlOptions = (serviceProvider, options) => options.UseNpgsql(connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));

        services.AddDbContext<ApplicationDbContext>(sqlOptions);
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        return services;
    }
}