using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.Data.Validation;

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
        
        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .BuildEntity()
            .AddJsonEntity<BranchOrder>()
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();
        
        JsonContext.SetContext(ctx);
    }

    [Fact]
    public void CreateValidBranchOrder()
    {
        ResetContext();

        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        var today = DateTime.Today;

        var order = new BranchOrder(
            1,
            today,
            new Dictionary<Product, int> { { product, 5 } },
            today.AddDays(1),
            "Warehouse A",
            "Store B"
        );

        Assert.Equal("Warehouse A", order.From);
        Assert.Equal("Store B", order.To);
        Assert.Contains(order, BranchOrder.All);
    }

    [Fact]
    public void CreateBranchOrderSameFromTo()
    {
        ResetContext();

        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        var today = DateTime.Today;

        var exception = Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                1,
                today,
                new Dictionary<Product, int> { { product, 5 } },
                today.AddDays(1),
                "Store A",
                "Store A"
            );
        });

        Assert.Contains("cannot be the same", exception.Message);
    }

    [Fact]
    public void CreateBranchOrderWithEmptyFrom()
    {
        ResetContext();
        
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                1, DateTime.Today, new Dictionary<Product, int> { { product, 1 } }, DateTime.Today.AddDays(1),
                "",
                "Store B"
            );
        });
    }

    [Fact]
    public void CreateBranchOrderWithEmptyTo()
    {
        ResetContext();
        
        var images = new DeserializableReadOnlyList<string>(new List<string> { "image.png" }.AsReadOnly());
        var product = new Product("P1", "D1", 10m, images, 1m, new Dimensions(1,1,1));
        
        Assert.Throws<ValidationException>(() =>
        {
            new BranchOrder(
                1, DateTime.Today, new Dictionary<Product, int> { { product, 1 } }, DateTime.Today.AddDays(1),
                "Store A",
                ""
            );
        });
    }
}