using Microsoft.Extensions.DependencyInjection;

namespace BYTPRO.Data;

public static class ServiceCollectionDataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        // services.AddScoped<IService, Service>();
        return services;
    }
}