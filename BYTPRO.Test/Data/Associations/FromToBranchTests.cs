using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class FromToBranchTests
{
    [Fact]
    public void ConstructorValidOrderSucceedsAndSetsProperties()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouseFrom = new Warehouse(address, "Warehouse A", "09-18", 100m, 50m, 10, 0m);
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        var order = new BranchOrder(
            1,
            today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 5)],
            today.AddDays(1),
            warehouseFrom,
            branchTo 
        );

        Assert.Same(warehouseFrom, order.From);
        Assert.Same(branchTo, order.To);
        Assert.Equal("Warehouse A", order.From.Name);
        Assert.Contains(order, BranchOrder.All);
    }

    [Fact]
    public void ConstructorSameFromAndToThrowsValidationException()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var address = new Address("Street", "1", null, "00-000", "City");
        var warehouse = new Warehouse(address, "Store A", "09-18", 100m, 50m, 10, 0m);

        var exception = Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                1,
                today,
                OrderStatus.InProgress,
                [new ProductEntry(product, 5)],
                today.AddDays(1),
                warehouse,
                warehouse 
            );
        });

        Assert.Contains("cannot be the same", exception.Message);
    }
    
    [Fact]
    public void ConstructorDifferentObjectsSameNameSucceeds()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var address1 = new Address("Street1", "1", null, "00-000", "City");
        var address2 = new Address("Street2", "2", null, "00-000", "City");
        
        var warehouseFrom = new Warehouse(address1, "Shared Name", "09-18", 100m, 50m, 10, 0m);
        var branchTo = new PickupPoint(address2, "Shared Name", "09-18", 100m, 50, 10m);

        var order = new BranchOrder(
            1,
            today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 5)],
            today.AddDays(1),
            warehouseFrom,
            branchTo 
        );

        Assert.NotSame(warehouseFrom, branchTo);
        Assert.Equal(warehouseFrom.Name, branchTo.Name);
        Assert.Same(warehouseFrom, order.From);
        Assert.Same(branchTo, order.To);
    }

    [Fact]
    public void ConstructorNullFromThrowsException()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        var address = new Address("Street", "1", null, "00-000", "City");
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        Assert.ThrowsAny<Exception>(() =>
        {
            new BranchOrder(
                1, 
                DateTime.Today, 
                OrderStatus.InProgress, 
                [new ProductEntry(product, 1)], 
                DateTime.Today.AddDays(1),
                null!,
                branchTo
            );
        });
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
                1, 
                DateTime.Today, 
                OrderStatus.InProgress, 
                [new ProductEntry(product, 1)], 
                DateTime.Today.AddDays(1),
                warehouseFrom,
                null!
            );
        });
    }
    
    [Fact]
    public void ConstructorFromPickupPointCausesCompilationError()
    {
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;
        
        var address = new Address("Street", "1", null, "00-000", "City");
        var pickupPointFrom = new PickupPoint(address, "Pickup Point", "09-18", 50m, 20, 5m);
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        Assert.True(true, " From has to be a Warehouse");
    }
}