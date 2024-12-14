using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure.Auth;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var JWT_OPTIONS = configuration.GetSection(("Jwt")); 
        services.Configure<JwtOptions>(JWT_OPTIONS);
        services.AddSingleton<JwtManager>();
        return services;
    }
}