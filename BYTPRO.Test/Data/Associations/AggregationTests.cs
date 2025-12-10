using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

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

    private static Product CreateProduct(string name)
    {
        var imagesList = new DeserializableReadOnlyList<string>(new List<string> { "img.png" }.AsReadOnly());
        
        return new Product(
            name,
            "Description",
            5.0m,
            imagesList,
            1.0m,
            new Dimensions(10, 10, 20),
            null
        );
    }

    [Fact]
    public void CloseBranchWithStockThrowsExceptionAndNotClose()
    {
        var branch = new TestBranch("Stock Branch");
        var product = CreateProduct("Milk");

        branch.AddProductStock(product, 50);

        Assert.NotEmpty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        Assert.Throws<InvalidOperationException>(() => { branch.CloseBranch(); });

        Assert.Contains(branch, Branch.All);
    }

    [Fact]
    public void CloseBranchWithoutStockSucceedsAndRemovesFromExtent()
    {
        var branch = new TestBranch("Empty Branch");

        Assert.Empty(branch.Stocks);
        Assert.Contains(branch, Branch.All);

        branch.CloseBranch();

        Assert.DoesNotContain(branch, Branch.All);
    }
    
    [Fact]
    public void AddProductStockNewProductSetsBidirectionalLink()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Bread");

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
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Apples");

        branch.AddProductStock(product, 5);
        branch.AddProductStock(product, 15);

        Assert.Single(branch.Stocks);
        var stockItem = branch.Stocks.First();
        Assert.Equal(20, stockItem.Quantity);
        
        Assert.Single(product.StockedIn);
    }

    [Fact]
    public void AddProductStockZeroQuantityThrowsValidationException()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Cheese");

        Assert.Throws<ValidationException>(() => branch.AddProductStock(product, 0));
        
        Assert.Empty(branch.Stocks);
    }
    
    [Fact]
    public void ReduceProductStockSucceedsAndDecrementsQuantity()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Water");

        branch.AddProductStock(product, 100);
        
        branch.ReduceStockForItems([new ProductEntry(product, 30)]);
        
        var stockItem = branch.Stocks.First();
        Assert.Equal(70, stockItem.Quantity);
    }

    [Fact]
    public void ReduceProductStockInsufficientQuantityThrowsInvalidOperationException()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Soda");

        branch.AddProductStock(product, 5);
        
        Assert.Throws<InvalidOperationException>(() => 
            branch.ReduceStockForItems([new ProductEntry(product, 10)]));
        
        Assert.Equal(5, branch.Stocks.First().Quantity);
    }
    
    [Fact]
    public void ReduceProductStockNonExistingProductThrowsInvalidOperationException()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Coffee");
        var missingProduct = CreateProduct("Tea");
        
        branch.AddProductStock(product, 5);
        
        Assert.Throws<InvalidOperationException>(() => 
            branch.ReduceStockForItems([new ProductEntry(missingProduct, 1)]));
    }
    
    [Fact]
    public void BranchProductStockConstructorNegativeQuantityThrowsValidationException()
    {
        var branch = new TestBranch("Test Branch");
        var product = CreateProduct("Sugar");

        Assert.Throws<ValidationException>(() => 
            new BranchProductStock(branch, product, -1));
    }
}