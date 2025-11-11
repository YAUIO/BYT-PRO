using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
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