using Microsoft.Extensions.DependencyInjection;

namespace BYTPRO.Core;

public static class ServiceCollectionCoreExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // services.AddScoped<IService, Service>();
        return services;
    }
}