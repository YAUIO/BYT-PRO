using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedTests
{
    private static Product CreateTestProduct()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        return new Product(
            "TestProduct",
            "Description",
            100m,
            images,
            1.5m,
            new Dimensions(10, 10, 10)
        );
    }
    
    private static Customer CreateTestCustomer(int id, string trackingPrefix)
    {
        return new Customer(
            id,
            "Name",
            "Surname",
            "+123456789",
            $"{trackingPrefix}@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );
    }

    private static PickupPoint CreateTestPickupPoint(string name)
    {
        var address = new Address("St", "1", null, "00", "City");
        return new PickupPoint(address, name, "09-18", 50, 100, 20);
    }


    [Fact]
    public void ConstructorValidOrderSucceedsAndSetsProperties()
    {
        var pickupPoint = CreateTestPickupPoint("Point A");
        var customer = CreateTestCustomer(101, "alice");
        var product = CreateTestProduct();

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
    public void ConstructorTrackingNumberAlreadyExistsThrowsValidationException()
    {
        var pickupPoint = CreateTestPickupPoint("Point B");
        var customer = CreateTestCustomer(1002, "john");
        var product = CreateTestProduct();

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

        Assert.True(customer.OnlineOrders.ContainsKey("TRACK-ABC-12345"));
        Assert.True(customer.OnlineOrders.ContainsValue(order1));

        Assert.Throws<ValidationException>(() =>
        {
            new OnlineOrder(
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

    [Fact]
    public void ConstructorNullCustomerThrowsException()
    {
        var pickupPoint = CreateTestPickupPoint("Point C");
        var product = CreateTestProduct();

        Assert.ThrowsAny<Exception>(() =>
        {
            new OnlineOrder(
                204,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                true,
                null,
                "TRACK-NULL-CUSTOMER",
                null!,
                pickupPoint
            );
        });
    }

    [Fact]
    public void ConstructorNullPickupPointThrowsException()
    {
        var customer = CreateTestCustomer(1003, "dave");
        var product = CreateTestProduct();

        Assert.ThrowsAny<Exception>(() =>
        {
            new OnlineOrder(
                205,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                true,
                null,
                "TRACK-NULL-POINT",
                customer,
                null!
            );
        });
    }

    [Fact]
    public void ConstructorNullTrackingNumberThrowsException()
    {
        var pickupPoint = CreateTestPickupPoint("Point D");
        var customer = CreateTestCustomer(1004, "eve");
        var product = CreateTestProduct();

        Assert.Throws<ValidationException>(() =>
        {
            new OnlineOrder(
                206,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                true,
                null,
                null!,
                customer,
                pickupPoint
            );
        });
    }
}