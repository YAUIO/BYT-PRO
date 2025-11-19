using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
using Microsoft.Extensions.DependencyInjection;

namespace BYTPRO.Data;

public static class ServiceCollectionDataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        var context = new JsonContextBuilder()
            // ----------< People >----------
            .AddJsonEntity<Customer>()
            .WithFileName("customers")
            .BuildEntity()
            // ------------------------------
            .AddJsonEntity<LocalEmployee>()
            .WithFileName("localEmployees")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<RegionalEmployee>()
            .WithFileName("regionalEmployees")
            .BuildEntity()

            // ----------< Sales >----------
            .AddJsonEntity<OnlineOrder>()
            .WithFileName("onlineOrders")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<OfflineOrder>()
            .WithFileName("offlineOrders")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<BranchOrder>()
            .WithFileName("branchOrders")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<Product>()
            .WithFileName("products")
            .BuildEntity()

            // ----------< Locations >----------
            .AddJsonEntity<PickupPoint>()
            .WithFileName("pickupPoints")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<Store>()
            .WithFileName("stores")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<Warehouse>()
            .WithFileName("warehouses")
            .BuildEntity()

            //------------------------------
            .WithRoot(new DirectoryInfo($"{Directory.GetCurrentDirectory()}/Db"))
            .Build();

        JsonContext.SetContext(context);

        services.AddSingleton(context);

        return services;
    }
}