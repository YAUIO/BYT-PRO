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
            .AddJsonEntity<LocalEmployee>()
            .AddJsonEntity<RegionalEmployee>()

            // ----------< Locations >----------
            .AddJsonEntity<PickupPoint>()
            .AddJsonEntity<Store>()
            .AddJsonEntity<Warehouse>()

            // ----------< Sales >----------
            .AddJsonEntity<Product>()
            .AddJsonEntity<OnlineOrder>()
            .AddJsonEntity<OfflineOrder>()
            .AddJsonEntity<BranchOrder>()

            //------------------------------
            .BuildWithDbRoot($"{Directory.GetCurrentDirectory()}/Db.json");

        JsonContext.SetContext(context);

        services.AddSingleton(context);

        return services;
    }
}