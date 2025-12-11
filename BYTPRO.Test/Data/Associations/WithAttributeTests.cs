using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;

namespace BYTPRO.Test.Data.Associations;

public class WithAttributeTests
{
    private static readonly Address TestAddress = new("Street", "1", null, "00-000", "City");
    private static readonly Warehouse WarehouseA = new(TestAddress, "Warehouse A", "09-17", 100m, 50m, 10);
    private static readonly Branch BranchB = new PickupPoint(TestAddress, "Store B", "09-18", 100m, 50, 10m);

    [Fact]
    public void TestBranchOrderCreation()
    {
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

        var order = new BranchOrder(
            301,
            DateTime.Today,
            OrderStatus.InProgress,
            [new ProductEntry(product1, 3), new ProductEntry(product2, 5)],
            DateTime.Today.AddDays(1),
            WarehouseA,
            BranchB
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

        Assert.Throws<ValidationException>(() =>
        {
            var order = new BranchOrder(
                302,
                DateTime.Today,
                OrderStatus.InProgress,
                [new ProductEntry(product3, 1), new ProductEntry(product1, -1)],
                DateTime.Today.AddDays(1),
                WarehouseA,
                BranchB
            );
        });
    }
}