using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BYTPRO.Data;

public static class ServiceCollectionDataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        var context = new JsonContextBuilder()
            .AddJsonEntity<Person>()
                .WithFileName("person")
                .BuildEntity()
            .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(""))
            .Build();
        
        services.AddSingleton(context);

        services.AddScoped<IUnitOfWork, JsonUnitOfWork>();
        
        return services;
    }
}