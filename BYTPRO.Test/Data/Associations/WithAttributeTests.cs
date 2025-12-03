using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class WithAttributeTests
{
    /*4. Association with attribute: Order <-> Product
    По скольку Order abstract, то тесить выйдет только на child classes, 
    сейчас это лучше всего делать на BranchOrder (он пока что не имеет зависимостей как другие) 
    и можно неплохо потестить сам base class Order, а именно:
    Различное создание (valid, invalid) по атрибуту Dictionary<Product, int> orderItems 
    и в случае успешного создания, проверить его OrderItems.*/
    
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestWAttributeDb";

    private static void ResetContext(bool clearContext = true)
    {
        if (Directory.Exists(DbRoot) && clearContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        var ctx= new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .BuildEntity()
            .AddJsonEntity<BranchOrder>()
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();
        
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

        Order order = new BranchOrder(1,
            today,
            new () {
                {product3, 1},
                {product1, 2}
            },
            today.AddDays(1)
        );
        
        await JsonContext.Context.SaveChangesAsync();

        var items = order.OrderItems
            .Select(s => s.Product)
            .ToHashSet();

        Assert.Contains(order, Order.All);
        Assert.Contains(product3, items);
    }
}