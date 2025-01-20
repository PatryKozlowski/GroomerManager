using Azure.Storage.Blobs;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Infrastructure.Auth;
using GroomerManager.Infrastructure.Persistence.Configurations;
using GroomerManager.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroomerManager.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration.GetConnectionString("GroomerManagerStore")!);
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));
        services.AddTransient<IDateTime, DataTimeService>();
        services.AddJwt(configuration);
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddSingleton<IBlobService, BlobService>();
        return services;
    }
}