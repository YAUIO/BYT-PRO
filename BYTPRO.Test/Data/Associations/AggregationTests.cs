using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class AggregationTests
{
    [Fact]
    public void CloseBranchWithStockThrowsExceptionAndNotClose()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();

        branch.AddProductStock(product, 50);

        Assert.NotEmpty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        Assert.Throws<InvalidOperationException>(() => { branch.CloseBranch(); });

        Assert.Contains(branch, Branch.All);
    }

    [Fact]
    public void CloseBranchWithoutStockSucceedsAndRemovesFromExtent()
    {
        var branch = LocationsFactory.CreatePureBranch();

        Assert.Empty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        branch.CloseBranch();

        Assert.DoesNotContain(branch, Branch.All);
    }

    [Fact]
    public void AddProductStockNewProductSetsBidirectionalLink()
    {
        var product = SalesFactory.CreateProduct();
        var branch = LocationsFactory.CreatePureBranch();

        branch.AddProductStock(product, 10);

        Assert.Single(branch.Stocks);
        var stockItem = branch.Stocks.First();

        Assert.Same(branch, stockItem.Branch);
        Assert.Contains(stockItem, branch.Stocks);

        Assert.Same(product, stockItem.Product);
        Assert.Contains(stockItem, product.StockedIn);
        Assert.Equal(10, stockItem.Quantity);
    }

    [Fact]
    public void AddProductStockExistingProductIncrementsQuantity()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();

        branch.AddProductStock(product, 5);
        branch.AddProductStock(product, 15);

        Assert.Single(branch.Stocks);
        var stockItem = branch.Stocks.First();
        Assert.Equal(20, stockItem.Quantity);

        Assert.Single(product.StockedIn);
    }

    [Fact]
    public void ReduceProductStockSucceedsAndDecrementsQuantity()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();

        branch.AddProductStock(product, 100);

        branch.ReduceStockForItems([new ProductEntry(product, 30)]);

        var stockItem = branch.Stocks.First();
        Assert.Equal(70, stockItem.Quantity);
    }

    [Fact]
    public void ReduceProductStockInsufficientQuantityThrowsInvalidOperationException()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();

        branch.AddProductStock(product, 5);

        Assert.Throws<InvalidOperationException>(() =>
            branch.ReduceStockForItems([new ProductEntry(product, 10)]));

        Assert.Equal(5, branch.Stocks.First().Quantity);
    }

    [Fact]
    public void ReduceProductStockNonExistingProductThrowsInvalidOperationException()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();
        var missingProduct = SalesFactory.CreateProduct();

        branch.AddProductStock(product, 5);

        Assert.Throws<InvalidOperationException>(() =>
            branch.ReduceStockForItems([new ProductEntry(missingProduct, 1)]));
    }

    [Fact]
    public void BranchProductStockConstructorNegativeQuantityThrowsValidationException()
    {
        var branch = LocationsFactory.CreatePureBranch();
        var product = SalesFactory.CreateProduct();

        Assert.Throws<ValidationException>(() =>
            new BranchProductStock(branch, product, -1));
    }
}