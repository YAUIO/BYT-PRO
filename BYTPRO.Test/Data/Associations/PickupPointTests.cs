using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class PickupPointTests
{
    [Fact]
    public void CreateOnlineOrder()
    {
        var customer = new Customer(1, "Alice", "Test", "123456789", "alice@test.com", "pass", DateTime.Now);
        
        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        var product = new Product("Prod1", "Desc", 100m, images, 1m, new Dimensions(1,1,1));

        var address = new Address("Street", "1", null, "00-000", "City");
        var pickupPoint = new PickupPoint(address, "Point A", "09:00-20:00", 50m, 100, 20m);

        var order = new OnlineOrder(
            101,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product, 1)],
            true,
            null,
            "TRACK-1",
            customer,
            pickupPoint
        );

        Assert.NotNull(order.PickupPoint);
        Assert.Same(pickupPoint, order.PickupPoint);

        Assert.Contains(order, customer.OnlineOrders.Values);
        Assert.Contains(order, pickupPoint.OnlineOrders);
    }

    [Fact]
    public void CreateMultipleOrders()
    {
        var customer = new Customer(1, "Bob", "Test", "987654321", "bob@test.com", "pass", DateTime.Now);
        
        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        var product = new Product("Prod1", "Desc", 100m, images, 1m, new Dimensions(1,1,1));
        
        var address = new Address("Street", "2", null, "00-000", "City");
        var pickupPoint = new PickupPoint(address, "Point B", "10:00-20:00", 100m, 200, 20m);

        var order1 = new OnlineOrder(201, DateTime.Now, OrderStatus.InProgress, [new ProductEntry(product, 1)], true, null, "T1", customer, pickupPoint);
        var order2 = new OnlineOrder(202, DateTime.Now, OrderStatus.InProgress, [new ProductEntry(product, 1)], true, null, "T2", customer, pickupPoint);

        Assert.Equal(2, pickupPoint.OnlineOrders.Count);
        Assert.Contains(order1, pickupPoint.OnlineOrders);
        Assert.Contains(order2, pickupPoint.OnlineOrders);
    }

    [Fact]
    public void CreateOnlineOrderWithNullPickupPoint()
    {
        var customer = new Customer(1, "Alice", "Test", "123456789", "alice@test.com", "pass", DateTime.Now);
        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        var product = new Product("Prod1", "Desc", 100m, images, 1m, new Dimensions(1,1,1));

        Assert.ThrowsAny<Exception>(() =>
        {
            new OnlineOrder(
                101,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                true,
                null,
                "TRACK-NULL-TEST",
                customer,
                null!
            );
        });
    }
}