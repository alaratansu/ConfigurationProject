using Microsoft.Extensions.DependencyInjection;
using Configuration.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Configuration.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IRedisCacheService, RedisCacheService>();

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = $"{config["Redis:Host"]}:{config["Redis:Port"]}";
        });

        return services;
    }
}