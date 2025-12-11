using System.Collections;
using System.Text;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using BYTPRO.Test.Data.Factories;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase(ITestOutputHelper console, JsonContext context)
{
    [Fact]
    private void CreateSampleEntities()
    {
        // ----------< Locations >----------
        var pickupPoint = LocationsFactory.CreatePickupPoint();
        var store = LocationsFactory.CreateStore();
        LocationsFactory.CreateWarehouse();

        // ----------< People >----------
        var customer = PeopleFactory.CreateCustomer();
        PeopleFactory.CreateLocalEmployee(store);
        PeopleFactory.CreateRegionalEmployee();

        // ----------< Sales >----------
        var product1 = SalesFactory.CreateProduct();
        var product2 = SalesFactory.CreateProduct();

        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);
        store.AddProductStock(product2, 10);


        SalesFactory.CreateOnlineOrder(
            cart:
            [
                new ProductEntry(product1, 1),
                new ProductEntry(product2, 5)
            ],
            customer,
            pickupPoint
        );

        SalesFactory.CreateOfflineOrder(
            cart:
            [
                new ProductEntry(product1, 2),
                new ProductEntry(product2, 2)
            ],
            store
        );

        context.SaveChanges();
    }

    [Fact]
    private void OutputAllExtents()
    {
        var sections = new (string Title, IEnumerable Data)[]
        {
            ("Locations", new (string, IEnumerable)[]
            {
                ("Branches", Branch.All),
                ("PickupPoints", PickupPoint.All),
                ("Stores", Store.All),
                ("Warehouses", Warehouse.All)
            }),
            ("People", new (string, IEnumerable)[]
            {
                ("Persons", Person.All),
                ("Customers", Customer.All),
                ("Employees", Employee.All),
                ("LocalEmployees", LocalEmployee.All),
                ("RegionalEmployees", RegionalEmployee.All)
            }),
            ("Sales", new (string, IEnumerable)[]
            {
                ("Products", Product.All),
                ("Orders", Order.All),
                ("OnlineOrders", OnlineOrder.All),
                ("OfflineOrders", OfflineOrder.All),
                ("BranchOrders", BranchOrder.All)
            })
        };

        var sb = new StringBuilder();

        foreach (var (title, dataArray) in sections)
        {
            sb.AppendLine($"\n\n--------------------> {title} <--------------------");

            foreach (var (name, data) in dataArray.Cast<(string, IEnumerable)>())
            {
                sb.AppendLine($"{name}({data.Cast<object>().Count()}): {data.ToJson()}");
                sb.AppendLine("\n----------------------------------------\n");
            }
        }

        console.WriteLine(sb.ToString());
    }

    [Fact]
    public void DeleteDatabaseFile()
    {
        console.WriteLine("Deleting (if exists): " + context.DbPath);
        File.Delete(context.DbPath);
    }
}