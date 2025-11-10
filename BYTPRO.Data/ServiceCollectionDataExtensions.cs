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
            .WithRoot(new DirectoryInfo($"{Directory.GetCurrentDirectory()}/Db"))
            .Build();
        
        JsonContext.SetContext(context);
        
        services.AddSingleton(context);
        
        services.AddSingleton<IUnitOfWork, JsonUnitOfWork>();
        
        return services;
    }
}