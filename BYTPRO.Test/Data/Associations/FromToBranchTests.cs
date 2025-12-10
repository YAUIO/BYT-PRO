using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class FromToBranchTests
{
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
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        return new Product(name, "D1", 10m, images, 1m, new Dimensions(1,1,1));
    }

    [Fact]
    public void ConstructorValidOrderWarehouseToPickupPointSucceeds()
    {
        var product = CreateProduct("P1");
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Warehouse A", "09-18", 100m, 50m, 10, 0m);
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        var order = new BranchOrder(
            101,
            DateTime.Today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 5)],
            DateTime.Today.AddDays(1),
            warehouseFrom,
            branchTo 
        );

        Assert.Same(warehouseFrom, order.From);
        Assert.Same(branchTo, order.To);
        Assert.Equal("Warehouse A", order.From.Name);
        Assert.Contains(order, BranchOrder.All);
    }
    
    [Fact]
    public void ConstructorValidOrderWarehouseToStoreSucceeds()
    {
        var product = CreateProduct("P2");
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Warehouse B", "09-18", 100m, 50m, 10, 0m);
        var storeTo = new TestStore("Retail Store");

        var order = new BranchOrder(
            102,
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
        var warehouseFrom = new Warehouse(address, "WH From", "09-18", 100m, 50m, 10, 0m);
        var warehouseTo = new Warehouse(address, "WH To", "09-18", 100m, 50m, 10, 0m);

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
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Store A", "09-18", 50m, 20m, 5, 0m);

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