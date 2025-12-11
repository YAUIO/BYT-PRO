using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class BasicTests
{
    // Store <-> OfflineOrder

    [Fact]
    public void TestCreateOfflineOrder()
    {
        var product1 = SalesFactory.CreateProduct();
        var product2 = SalesFactory.CreateProduct();

        var store = LocationsFactory.CreateStore();
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);
        store.AddProductStock(product2, 10);

        var offlineOrder = SalesFactory.CreateOfflineOrder(
            cart:
            [
                new ProductEntry(product1, 2),
                new ProductEntry(product2, 2)
            ],
            store
        );

        // Check associations being created
        Assert.Equal(offlineOrder.Store, store);
        Assert.Contains(offlineOrder, store.OfflineOrders);

        // Check stocks being updated (business logic)
        Assert.Equal(3, store.Stocks.Single(s => s.Product == product1).Quantity);
        Assert.Equal(8, store.Stocks.Single(s => s.Product == product2).Quantity);
    }


    // PickupPoint <-> OnlineOrder

    [Fact]
    public void TestCreateOnlineOrder()
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
        Assert.Equal(onlineOrder.Customer, customer);
        Assert.Equal(onlineOrder.PickupPoint, pickupPoint);
        Assert.Contains(onlineOrder, pickupPoint.OnlineOrders);
    }


    // Branch <-> BranchOrder

    [Fact]
    public void ConstructorValidOrderWarehouseToPickupPointSucceeds()
    {
        var product = SalesFactory.CreateProduct();
        var warehouseFrom = LocationsFactory.CreateWarehouse();
        var branchTo = LocationsFactory.CreatePickupPoint();

        var order = SalesFactory.CreateBranchOrder(
            [new ProductEntry(product, 5)],
            warehouseFrom,
            branchTo
        );

        Assert.Same(warehouseFrom, order.From);
        Assert.Same(branchTo, order.To);
        Assert.Contains(order, BranchOrder.All);
    }

    [Fact]
    public void ConstructorValidOrderWarehouseToStoreSucceeds()
    {
        var product = SalesFactory.CreateProduct();
        var warehouseFrom = LocationsFactory.CreateWarehouse();
        var storeTo = LocationsFactory.CreateStore();

        var order = SalesFactory.CreateBranchOrder(
            [new ProductEntry(product, 2)],
            warehouseFrom,
            storeTo
        );

        Assert.Same(warehouseFrom, order.From);
        Assert.Same(storeTo, order.To);
        Assert.Contains(order, BranchOrder.All);
    }

    [Fact]
    public void ConstructorWarehouseToWarehouseThrowsValidationException()
    {
        var product = SalesFactory.CreateProduct();
        var warehouseFrom = LocationsFactory.CreateWarehouse();
        var warehouseTo = LocationsFactory.CreateWarehouse();

        var exception = Assert.Throws<ValidationException>(() =>
        {
            SalesFactory.CreateBranchOrder(
                [new ProductEntry(product, 1)],
                warehouseFrom,
                warehouseTo
            );
        });

        Assert.Contains("must be a Store or a PickupPoint", exception.Message);
    }
}