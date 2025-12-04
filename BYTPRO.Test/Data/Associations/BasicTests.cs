using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class BasicTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestReflexDb";

    private static void ResetContext(bool removeContext = true)
    {
        if (Directory.Exists(DbRoot) && removeContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        var ctx= new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .BuildEntity()
            .AddJsonEntity<OfflineOrder>()
            .BuildEntity()
            .AddJsonEntity<Store>()
            .BuildEntity()
            .AddJsonEntity<Customer>()
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();
        
        JsonContext.SetContext(ctx);
    }

    [Fact]
    private void TestCreate()
    {
        ResetContext();
        
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );
        
        var customer = new Customer(
            102,
            "Artiom",
            "Bezkorovainyi",
            "+48000000001",
            "s30001@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );
        
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);
        
        var offlineOrder = new OfflineOrder(
            4,
            DateTime.Now,
            new Dictionary<Product, int>
            {
                { product1, 2 },
                { product2, 2 }
            },
            null,
            store
        );
    } 
    
    [Fact]
    private void TestCreateChangesStock()
    {
        ResetContext();
        
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );
        
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);
        
        var offlineOrder = new OfflineOrder(
            8,
            DateTime.Now,
            new Dictionary<Product, int>
            {
                { product1, 2 },
                { product2, 2 }
            },
            null,
            store
        );
        
        Assert.Equal(3, store.Stocks.Single(s => s.Product.Name.Equals(product1.Name)).Quantity);
        Assert.Equal(8, store.Stocks.Single(s => s.Product.Name.Equals(product2.Name)).Quantity);
    }
    
    [Fact]
    private void TestCreateFailsIfNoProductIsPresent()
    {
        ResetContext();
        
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );
        
        var customer = new Customer(
            4,
            "Artiom",
            "Bezkorovainyi",
            "+48000000003",
            "s30003@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );
        
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);
        
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );

        Assert.Throws<ValidationException>(() =>
        {
            var offlineOrder = new OfflineOrder(
                2,
                DateTime.Now,
                new Dictionary<Product, int>
                {
                    { product1, 2 },
                    { product3, 2 }
                },
                null,
                store
            );
        });
    } 
    
    [Fact]
    private void TestCreateFailsIfNotEnoughQuantityInStock()
    {
        ResetContext();
        
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );
        
        var customer = new Customer(
            6,
            "Artiom",
            "Bezkorovainyi",
            "+48000000004",
            "s30004@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );
        
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);

        Assert.Throws<ValidationException>(() =>
        {
            var offlineOrder = new OfflineOrder(
                2,
                DateTime.Now,
                new Dictionary<Product, int>
                {
                    { product1, 10 },
                    { product2, 2 }
                },
                null,
                store
            );
        });
    } 
}