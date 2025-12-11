using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class QualifiedTests
{
    [Fact]
    public void TestCreateOnlineOrderWithQualifiedAssociations()
    {
        var product = SalesFactory.CreateProduct();
        var customer = PeopleFactory.CreateCustomer();
        var pickupPoint = LocationsFactory.CreatePickupPoint();

        var onlineOrder = SalesFactory.CreateOnlineOrder(
            [new ProductEntry(product, 2)],
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
        var product = SalesFactory.CreateProduct();
        var customer = PeopleFactory.CreateCustomer();
        var pickupPoint = LocationsFactory.CreatePickupPoint();

        var order1 = SalesFactory.CreateOnlineOrder(
            [new ProductEntry(product, 2)],
            customer,
            pickupPoint
        );

        Assert.Throws<ValidationException>(() =>
        {
            SalesFactory.CreateOnlineOrder(
                [new ProductEntry(product, 5)],
                customer,
                pickupPoint,
                trackingNumber: order1.TrackingNumber
            );
        });
    }
}