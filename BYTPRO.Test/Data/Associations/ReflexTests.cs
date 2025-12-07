using BYTPRO.Data.Models.Sales;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Data.Associations;

public class ReflexTests (ITestOutputHelper console)
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestReflexDb";

    private static void ResetContext(bool removeContext = true)
    {
        if (Directory.Exists(DbRoot) && removeContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        var ctx= new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .WithDbFile(new FileInfo(DbRoot))
            .Build();
        
        JsonContext.SetContext(ctx);
    }
    
    [Fact]
    public async Task TestCreateAndSaveProductOfOtherProducts()
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

        var p3Name = "Product3";
        
        var product3 = new Product(
            p3Name,
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m),
            [product1, product2]
        );
        
        await JsonContext.Context.SaveChangesAsync();

        var testedProduct = Product.All.Single(p => p.Name == p3Name);
        
        Assert.Contains(product3, Product.All);
        Assert.NotNull(testedProduct.ConsistsOf);
        Assert.Contains(product1, testedProduct.ConsistsOf);
        Assert.Contains(product2, testedProduct.ConsistsOf);
    }
    
    [Fact]
    public async Task TestLoadProductOfOtherProducts()
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

        var p3Name = "Product3";
        
        var product3 = new Product(
            p3Name,
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m),
            [product1, product2]
        );

        await JsonContext.Context.SaveChangesAsync();

        ResetContext(false);
        
        console.WriteLine(Product.All.ToJson());

        var testedProduct = Product.All.Single(p => p.Name == p3Name);

        var consistsOf = testedProduct.ConsistsOf?
            .Select(p => p.Name)
            .ToHashSet();
        
        Assert.Contains(testedProduct, Product.All);
        Assert.NotNull(consistsOf);
        Assert.Contains(product1.Name, consistsOf);
        Assert.Contains(product2.Name, consistsOf);
    }
}