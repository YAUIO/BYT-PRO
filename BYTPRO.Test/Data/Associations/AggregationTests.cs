using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Associations;

public class AggregationTests
{
    public class TestBranch : Branch
    {
        public TestBranch(string name) : base(
            new Address("Street", "1", null, "00-000", "City"), 
            name,
            "09:00-17:00",
            100m)
        {
            RegisterBranch();
        }
    }

    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestAggregationDb";

    private static void ResetContext(bool removeContext = true)
    {
        if (Directory.Exists(DbRoot) && removeContext) 
            Directory.Delete(DbRoot, true);
        
        if (!Directory.Exists(DbRoot))
            Directory.CreateDirectory(DbRoot);
        
        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();
        
        JsonContext.SetContext(ctx);
    }

    [Fact]
    public void DeleteBranchWithStockShouldThrowExceptionAndNotDelete()
    {
        ResetContext();

        var branch = new TestBranch("Stock Branch");

        var product = new Product(
            "Milk",
            "Fresh Milk",
            5.0m,
            new DeserializableReadOnlyList<string>(new List<string> { "milk.png" }.AsReadOnly()),
            1.0m,
            new Dimensions(10, 10, 20)
        );

        branch.AddProductStock(product, 50);

        Assert.NotEmpty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        var exception = Assert.Throws<InvalidOperationException>(() => 
        {
            branch.Delete();
        });

        Assert.Equal("Redistribute stocks before deleting a branch.", exception.Message);

        Assert.Contains(branch, Branch.All);
    }
}