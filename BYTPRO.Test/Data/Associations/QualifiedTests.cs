using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedAssociationTest
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/BYT_PRO_TESTS/Qualified.json";

    private static void ResetContext(bool removeContext = true)
    {
        if (File.Exists(DbRoot) && removeContext)
            File.Delete(DbRoot);

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .AddJsonEntity<Customer>()
            .AddJsonEntity<OnlineOrder>()
            .BuildWithDbFile(new FileInfo(DbRoot));

        JsonContext.SetContext(ctx);
    }

    [Fact]
    public void TestCreateOnlineOrderShouldAddItToCustomerAccessibleByTrackingNumber()
    {
        ResetContext();

        var customer = new Customer(
            101,
            "Alice",
            "Wonderland",
            "+123456789",
            "alice@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );

        var product = new Product(
            "TestProduct",
            "Description",
            100m,
            ["img.png"],
            1.5m,
            new Dimensions(10, 10, 10)
        );

        const string trackingNumber = "TRACK-ABC-123";

        var order = new OnlineOrder(
            201,
            creationDate: DateTime.Now,
            status: OrderStatus.InProgress,
            cart: [new ProductEntry(product, 2)],
            isPaid: true,
            cancellationDate: null,
            trackingNumber: trackingNumber,
            customer: customer
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

        var customer = new Customer(
            1002,
            "John",
            "Smith",
            "+123456789",
            "john.smith@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );

        var product = new Product(
            "Product",
            "Description",
            100m,
            ["img.png"],
            1.5m,
            new Dimensions(10, 10, 10)
        );

        var order1 = new OnlineOrder(
            202,
            creationDate: DateTime.Now,
            status: OrderStatus.InProgress,
            cart: [new ProductEntry(product, 2)],
            isPaid: true,
            cancellationDate: null,
            trackingNumber: "TRACK-ABC-12345",
            customer: customer
        );

        Assert.Contains(order1, Order.All);
        Assert.Contains(order1, OnlineOrder.All);

        Assert.True(customer.OnlineOrders.ContainsKey("TRACK-ABC-12345"));
        Assert.True(customer.OnlineOrders.ContainsValue(order1));

        Assert.Throws<ValidationException>(() =>
        {
            var order2 = new OnlineOrder(
                203,
                creationDate: DateTime.Now,
                status: OrderStatus.InProgress,
                cart: [new ProductEntry(product, 5)],
                isPaid: false,
                cancellationDate: null,
                trackingNumber: "TRACK-ABC-12345",
                customer: customer
            );
        });
    }
}