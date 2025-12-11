using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class WithAttributeTests
{
    [Fact]
    public void TestBranchOrderCreation()
    {
        var product1 = SalesFactory.CreateProduct();
        var product2 = SalesFactory.CreateProduct();

        var fromWarehouse = LocationsFactory.CreateWarehouse();
        var toStore = LocationsFactory.CreateStore();

        var order = SalesFactory.CreateBranchOrder(
            cart:
            [
                new ProductEntry(product1, 3),
                new ProductEntry(product2, 5)
            ],
            fromWarehouse,
            toStore
        );

        // Check associations being created

        // Order contains all products
        var productsInOrder = order.AssociatedProducts
            .Select(s => s.Product)
            .ToHashSet();

        Assert.Contains(product1, productsInOrder);
        Assert.Contains(product2, productsInOrder);

        // Product1 is used in order
        var ordersWhereProduct1IsUsed = product1.AssociatedOrders
            .Select(s => s.Order)
            .ToHashSet();

        Assert.Contains(order, ordersWhereProduct1IsUsed);

        // Product2 is used in order
        var ordersWhereProduct2IsUsed = product2.AssociatedOrders
            .Select(s => s.Order)
            .ToHashSet();

        Assert.Contains(order, ordersWhereProduct2IsUsed);
    }


    // BELOW - VALIDATION ONLY TESTS (NO ASSOCIATIONS TESTS)

    [Fact]
    public void TestInvalidBranchOrderCreation()
    {
        var product1 = SalesFactory.CreateProduct();
        var product2 = SalesFactory.CreateProduct();
        var product3 = SalesFactory.CreateProduct(consistsOf: [product1, product2]);

        Assert.Throws<ValidationException>(() =>
        {
            SalesFactory.CreateBranchOrder(
                cart:
                [
                    new ProductEntry(product3, 1),
                    new ProductEntry(product1, -1)
                ],
                LocationsFactory.CreateWarehouse(),
                LocationsFactory.CreateStore()
            );
        });
    }
}