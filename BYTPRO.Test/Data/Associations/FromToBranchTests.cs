using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.Data.Validation;
using System.IO;

namespace BYTPRO.Test.Data.Associations;

public class FromToBranchTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestBranchOrderDb";

    private static void ResetContext(bool clearContext = true)
    {
        if (Directory.Exists(DbRoot) && clearContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        JsonContext.Context = null;

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .AddJsonEntity<PickupPoint>()
            .AddJsonEntity<BranchOrder>()
            .BuildWithDbRoot(Path.Combine(DbRoot, "test.json"));
        
        JsonContext.Context = ctx;
    }

    [Fact]
    public void CreateValidBranchOrder()
    {
        ResetContext();

        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var address = new Address("Street", "1", null, "00-000", "City");
        var branchFrom = new PickupPoint(address, "Warehouse A", "09-18", 100m, 50, 10m);
        var branchTo = new PickupPoint(address, "Store B", "09-18", 50m, 20, 5m);

        var order = new BranchOrder(
            1,
            today,
            OrderStatus.InProgress,
            [new ProductEntry(product, 5)],
            today.AddDays(1),
            branchFrom,
            branchTo 
        );

        // 4. Проверяем
        Assert.Same(branchFrom, order.From);
        Assert.Same(branchTo, order.To);
        Assert.Equal("Warehouse A", order.From.Name);
        Assert.Contains(order, BranchOrder.All);
    }

    [Fact]
    public void CreateBranchOrderSameFromTo()
    {
        ResetContext();

        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var address = new Address("Street", "1", null, "00-000", "City");
        var branch = new PickupPoint(address, "Store A", "09-18", 100m, 50, 10m);

        var exception = Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                1,
                today,
                OrderStatus.InProgress,
                [new ProductEntry(product, 5)],
                today.AddDays(1),
                branch,
                branch 
            );
        });

        Assert.Contains("cannot be the same", exception.Message);
    }

    [Fact]
    public void CreateBranchOrderWithNullFrom()
    {
        ResetContext();
        
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
    public void CreateBranchOrderWithNullTo()
    {
        ResetContext();
        
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        var address = new Address("Street", "1", null, "00-000", "City");
        var branchFrom = new PickupPoint(address, "Store A", "09-18", 50m, 20, 5m);

        Assert.ThrowsAny<Exception>(() =>
        {
            new BranchOrder(
                1, 
                DateTime.Today, 
                OrderStatus.InProgress, 
                [new ProductEntry(product, 1)], 
                DateTime.Today.AddDays(1),
                branchFrom,
                null!
            );
        });
    }
}