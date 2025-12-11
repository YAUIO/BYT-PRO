using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;

namespace BYTPRO.Test.Data.Associations;

public class BasicTests
{
    // Store <-> OfflineOrder

    [Fact]
    public void TestCreateOfflineOrder()
    {
        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
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
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);
        store.AddProductStock(product2, 10);

        var offlineOrder = new OfflineOrder(
            102,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product1, 2), new ProductEntry(product2, 2)],
            null,
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
        var customer = new Customer(
            9999,
            "Bob",
            "Test",
            "+987654321",
            "bob@test.com",
            "pass",
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

        var pickupPoint = new PickupPoint(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Point1",
            "09:00-22:00",
            50m,
            100,
            20m
        );

        var onlineOrder = new OnlineOrder(
            111,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product1, 2)],
            true,
            null,
            "TRACK-ABCDE-12345",
            customer,
            pickupPoint
        );

        // Check associations being created
        Assert.Equal(onlineOrder.Customer, customer);
        Assert.Equal(onlineOrder.PickupPoint, pickupPoint);
        Assert.Contains(onlineOrder, pickupPoint.OnlineOrders);
    }


    // Branch <-> BranchOrder

    private sealed class TestStore : Store
    {
        public TestStore(string name) : base(
            new Address("Street", "2", null, "00-000", "City"),
            name,
            "08-20",
            150m,
            3,
            100m,
            2)
        {
            FinishConstruction();
        }
    }

    private static Product CreateProduct(string name)
    {
        return new Product(name, "D1", 10m, ["image.png"], 1m, new Dimensions(1, 1, 1));
    }

    [Fact]
    public void ConstructorValidOrderWarehouseToPickupPointSucceeds()
    {
        var product = CreateProduct("P1");
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Warehouse A", "09-18", 100m, 50m, 10, 10m);
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        var order = new BranchOrder(
            501,
            DateTime.Today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 5)],
            DateTime.Today.AddDays(1),
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
        var product = CreateProduct("P2");
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Warehouse B", "09-18", 100m, 50m, 10, 10m);
        var storeTo = new TestStore("Retail Store");

        var order = new BranchOrder(
            502,
            DateTime.Today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 2)],
            DateTime.Today.AddDays(1),
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
        var product = CreateProduct("P3");
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "WH From", "09-18", 100m, 50m, 10, 10m);
        var warehouseTo = new Warehouse(address, "WH To", "09-18", 100m, 50m, 10, 10m);

        var exception = Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                103,
                DateTime.Today,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                DateTime.Today.AddDays(1),
                warehouseFrom,
                warehouseTo
            );
        });

        Assert.Contains("must be a Store or a PickupPoint", exception.Message);
    }

    [Fact]
    public void ConstructorNullToThrowsException()
    {
        var product = new Product("P1", "D1", 10m, ["image.png"], 1m, new Dimensions(1, 1, 1));

        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Store A", "09-18", 50m, 20m, 5, 10m);

        Assert.ThrowsAny<Exception>(() =>
        {
            new BranchOrder(
                107,
                DateTime.Today,
                OrderStatus.InProgress,
                [new ProductEntry(product, 1)],
                DateTime.Today.AddDays(1),
                warehouseFrom,
                null!
            );
        });
    }
}