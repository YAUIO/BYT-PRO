using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;

namespace BYTPRO.Test.Data.Associations;

public class AggregationTests
{
    private sealed class TestBranch : Branch
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

    [Fact]
    public void TestCloseBranchWithStockShouldThrowExceptionAndNotClose()
    {
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