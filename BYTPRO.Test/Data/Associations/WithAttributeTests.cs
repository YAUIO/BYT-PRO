using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class WithAttributeTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/BYT_PRO_TESTS/WithAttribute.json";

    private static void ResetContext(bool clearContext = true)
    {
        if (File.Exists(DbRoot) && clearContext)
            File.Delete(DbRoot);
        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .AddJsonEntity<BranchOrder>()
            .BuildWithDbRoot(DbRoot);

        JsonContext.SetContext(ctx);
    }

    [Fact]
    public async Task TestBranchOrderCreation()
    {
        ResetContext();

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

        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m),
            [product1, product2]
        );

        var today = DateTime.Today;

        Order order = new BranchOrder(
            301,
            today,
            OrderStatus.InProgress,
            [new ProductEntry(product3, 1), new ProductEntry(product1, 2)],
            today.AddDays(1)
        );

        await JsonContext.Context.SaveChangesAsync();

        var items = order.AssociatedProducts
            .Select(s => s.Product)
            .ToHashSet();

        Assert.Contains(order, Order.All);
        Assert.Contains(product3, items);
    }

    [Fact]
    public async Task TestInvalidBranchOrderCreation()
    {
        ResetContext();

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

        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m),
            [product1, product2]
        );

        var today = DateTime.Today;

        Assert.Throws<ValidationException>(() =>
        {
            Order order = new BranchOrder(
                302,
                today,
                OrderStatus.InProgress,
                [new ProductEntry(product3, 1), new ProductEntry(product1, -1)],
                today.AddDays(1)
            );
        });
    }
}