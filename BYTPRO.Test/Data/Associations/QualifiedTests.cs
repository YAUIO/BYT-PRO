using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;
using System.IO;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestQualifiedDb";

    private static void ResetContext(bool clearContext = true)
    {
        if (Directory.Exists(DbRoot) && clearContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        JsonContext.Context = null;

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .AddJsonEntity<Customer>()
            .AddJsonEntity<PickupPoint>()
            .AddJsonEntity<OnlineOrder>()
            .BuildWithDbRoot(Path.Combine(DbRoot, "test.json"));
        
        JsonContext.Context = ctx;
    }

    [Fact]
    public void TestCreateOnlineOrderShouldAddItToCustomerAccessibleByTrackingNumber()
    {
        ResetContext();

        var address = new Address("St", "1", null, "00", "City");
        var pickupPoint = new PickupPoint(address, "Point A", "09-18", 50, 100, 20);

        var customer = new Customer(
            101,
            "Alice",
            "Wonderland",
            "+123456789",
            "alice@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );

        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        var product = new Product(
            "TestProduct",
            "Description",
            100m,
            images,
            1.5m,
            new Dimensions(10, 10, 10)
        );

        const string trackingNumber = "TRACK-ABC-123";

        var order = new OnlineOrder(
            201,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product, 2)],
            true,
            null,
            trackingNumber,
            customer,
            pickupPoint
        );

        Assert.True(customer.OnlineOrders.ContainsKey(trackingNumber));

        var retrievedOrder = customer.OnlineOrders[trackingNumber];

        Assert.Same(order, retrievedOrder);
        Assert.Equal(trackingNumber, retrievedOrder.TrackingNumber);

        Assert.Same(customer, retrievedOrder.Customer);
    }

    [Fact]
    public void TestCreateOnlineOrderFailsIfTrackingNumberAlreadyExists()
    {
        ResetContext();

        var address = new Address("St", "1", null, "00", "City");
        var pickupPoint = new PickupPoint(address, "Point B", "09-18", 50, 100, 20);

        var customer = new Customer(
            1002,
            "John",
            "Smith",
            "+123456789",
            "john.smith@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );

        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        var product = new Product(
            "Product",
            "Description",
            100m,
            images,
            1.5m,
            new Dimensions(10, 10, 10)
        );

        var order1 = new OnlineOrder(
            202,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product, 2)],
            true,
            null,
            "TRACK-ABC-12345",
            customer,
            pickupPoint
        );

        Assert.Contains(order1, Order.All);
        Assert.Contains(order1, OnlineOrder.All);

        Assert.True(customer.OnlineOrders.ContainsKey("TRACK-ABC-12345"));
        Assert.True(customer.OnlineOrders.ContainsValue(order1));

        Assert.Throws<ValidationException>(() =>
        {
            var order2 = new OnlineOrder(
                203,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product, 5)],
                false,
                null,
                "TRACK-ABC-12345",
                customer,
                pickupPoint
            );
        });
    }
}