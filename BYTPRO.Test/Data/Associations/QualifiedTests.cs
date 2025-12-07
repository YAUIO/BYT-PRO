using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedAssociationTest
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestQualifiedDb";

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
    public void CreateOnlineOrderShouldAddItToCustomerAccessibleByTrackingNumber()
    {
        ResetContext();

        var customer = new Customer(
            id: 101,
            name: "Alice",
            surname: "Wonderland",
            phone: "+123456789",
            email: "alice@example.com",
            password: "password123",
            registrationDate: DateTime.Now.AddDays(-10)
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
            id: 101,
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
}