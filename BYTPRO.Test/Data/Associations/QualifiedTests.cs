using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedTests
{
    private static Product CreateProduct()
    {
        return new Product(
            "TestProduct",
            "Description",
            100m,
            ["img.png"],
            1.5m,
            new Dimensions(10, 10, 10)
        );
    }

    private static Customer CreateCustomer(string prefix)
    {
        return new Customer(
            Math.Abs(Guid.NewGuid().GetHashCode()),
            "Name",
            "Surname",
            "+123456789",
            $"{prefix}@example.com",
            "password123",
            DateTime.Now.AddDays(-10)
        );
    }

    private static PickupPoint CreatePickupPoint(string name)
    {
        return new PickupPoint(
            new Address("St", "1", null, "00", "City"),
            name,
            "09-18",
            50,
            100,
            20);
    }

    [Fact]
    public void TestCreateOnlineOrderWithQualifiedAssociations()
    {
        var product = CreateProduct();
        var customer = CreateCustomer("alice");
        var pickupPoint = CreatePickupPoint("Point A");

        var onlineOrder = new OnlineOrder(
            201,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product, 2)],
            true,
            null,
            "TRACK-ABC-123",
            customer,
            pickupPoint
        );

        // Check associations being created

        // Basic
        Assert.Equal(onlineOrder.Customer, customer);
        Assert.Equal(onlineOrder.PickupPoint, pickupPoint);
        Assert.Contains(onlineOrder, pickupPoint.OnlineOrders);

        // Qualified
        Assert.Contains(onlineOrder.TrackingNumber, customer.OnlineOrders.Keys);
        Assert.Contains(onlineOrder, customer.OnlineOrders.Values);
        Assert.Equal(onlineOrder, customer.OnlineOrders[onlineOrder.TrackingNumber]);
    }


    // BELOW - VALIDATION ONLY TESTS (NO ASSOCIATIONS TESTS)

    [Fact]
    public void TrackingNumberAlreadyExistsThrowsValidationException()
    {
        var product = CreateProduct();
        var customer = CreateCustomer("john");
        var pickupPoint = CreatePickupPoint("Point B");

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
        var pickupPoint = CreatePickupPoint("Point C");
        var product = CreateProduct();

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
        var customer = CreateCustomer("dave");
        var product = CreateProduct();

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
        var pickupPoint = CreatePickupPoint("Point D");
        var customer = CreateCustomer("eve");
        var product = CreateProduct();

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