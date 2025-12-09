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
            FinishConstruction();
        }
    }

    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/BYT_PRO_TESTS/Aggregation.json";

    private static void ResetContext(bool removeContext = true)
    {
        if (File.Exists(DbRoot) && removeContext)
            File.Delete(DbRoot);

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<Product>()
            .BuildWithDbRoot(DbRoot);

        JsonContext.SetContext(ctx);
    }

    [Fact]
    public void TestCloseBranchWithStockShouldThrowExceptionAndNotClose()
    {
        ResetContext();

        var branch = new TestBranch("Stock Branch");

        var product = new Product(
            "Milk",
            "Fresh Milk",
            5.0m,
            ["milk.png"],
            1.0m,
            new Dimensions(10, 10, 20)
        );

        branch.AddProductStock(product, 50);

        Assert.NotEmpty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        Assert.Throws<InvalidOperationException>(() => { branch.CloseBranch(); });

        Assert.Contains(branch, Branch.All);
    }
}