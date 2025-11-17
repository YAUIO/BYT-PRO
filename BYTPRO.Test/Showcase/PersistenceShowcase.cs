using System.Collections;
using System.Text;
using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase(ITestOutputHelper testOutputHelper)
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";

    static PersistenceShowcase()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);

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
            .AddJsonEntity<Product>()
            .WithFileName("products")
            .BuildEntity()
            //------------------------------
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
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();

        JsonContext.SetContext(context);
    }


    [Fact]
    public void TestClassExtent()
    {
        // ----------< People >----------
        var customer = new Customer(
            1,
            "Artiom",
            "Bezkorovainyi",
            "+48000000000",
            "s30000@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        var localEmployee = new LocalEmployee(
            2,
            "John",
            "Smith",
            "+48123456789",
            "john.smith@gmail.com",
            "12345",
            "12345678901",
            5000m,
            EmploymentType.FullTime,
            ["Basics"],
            "12:00-13:00"
        );

        var regionalEmployee = new RegionalEmployee(
            3,
            "Jane",
            "Smith",
            "+48123456780",
            "jane.smith@gmail.com",
            "123456789",
            "12345678902",
            10000m,
            EmploymentType.Intern,
            "INTERN@12345",
            SupervisionScope.Technical
        );


        // ----------< Sales >----------
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        var orderItem1 = new OrderItem(product1, 1);
        var orderItem2 = new OrderItem(product1, 5);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        var orderItem3 = new OrderItem(product2, 2);
        var orderItem4 = new OrderItem(product2, 10);


        // Orders cannot be properly tested now because of circular references with Product.
        // When any Order is saved, it is saved with all its Order Items, which includes Product.
        // And during deserialization, from each Order a new Product instance is created as well.
        // We might need to replace Product in OrderItem with some identifier (int: id).

        /*var onlineOrder = new OnlineOrder(
            1,
            DateTime.Now,
            [orderItem1],
            true,
            "TRN12345"
        );

        var offlineOrder = new OfflineOrder(
            2,
            DateTime.Now,
            [orderItem3, orderItem4],
            null
        );

        var branchOrder = new BranchOrder(
            3,
            DateTime.Now,
            [orderItem1, orderItem2, orderItem3, orderItem4],
            DateTime.Now.AddDays(2)
        );*/


        // ----------< Locations >----------
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );

        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );

        var warehouse = new Warehouse(
            new Address("Street3", "30/2", null, "01-456", "City3"),
            "Warehouse1",
            "00:00-24:00",
            10000m,
            50000m,
            3
        );

        JsonContext.Context.SaveChanges();

        ShowAll();
    }

    [Fact]
    public void ShowAll()
    {
        var sections = new (string Title, IEnumerable Data)[]
        {
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
                //("Orders", Order.All),
                //("OnlineOrders", OnlineOrder.All),
                //("OfflineOrders", OfflineOrder.All),
                //("BranchOrders", BranchOrder.All)
            }),
            ("Locations", new (string, IEnumerable)[]
            {
                ("Branches", Branch.All),
                ("PickupPoints", PickupPoint.All),
                ("Stores", Store.All),
                ("Warehouses", Warehouse.All)
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

        testOutputHelper.WriteLine(sb.ToString());
    }

    [Fact]
    public void TestReadOnly()
    {
        var emp = JsonContext.Context.GetTable<LocalEmployee>().First();
        Assert.Throws<NotSupportedException>(() => emp.TrainingsCompleted.Add("something-should-throw"));
        Assert.True(emp.TrainingsCompleted.IsReadOnly);
    }

    [Fact]
    public void Delete()
    {
        if (Directory.Exists(DbRoot)) Directory.Delete(DbRoot, true);
    }
}