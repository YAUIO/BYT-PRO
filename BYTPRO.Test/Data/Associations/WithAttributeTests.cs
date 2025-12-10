using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;

namespace BYTPRO.Test.Data.Associations;

public class WithAttributeTests
{
    private static readonly Address TestAddress = new("Street", "1", null, "00-000", "City");
    private static readonly Branch BranchA = new PickupPoint(TestAddress, "Warehouse A", "09-17", 100m, 50, 10m);
    private static readonly Branch BranchC = new PickupPoint(TestAddress, "Store C", "09-18", 100m, 50, 10m);
    
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

        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m),
            [product1, product2]
        );

        var order = new BranchOrder(
            301,
            DateTime.Today,
            OrderStatus.InProgress,
            [new ProductEntry(product3, 1), new ProductEntry(product1, 2)],
            DateTime.Today.AddDays(1),
            BranchA, 
            BranchC
        );

        var items = order.AssociatedProducts
            .Select(s => s.Product)
            .ToHashSet();

        Assert.Contains(order, Order.All);
        Assert.Contains(product3, items);
    }

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
                BranchA, 
                BranchC
            );
        });
    }
    
    [Fact]
    public void TestAssociateWithOrderAddsToProducts()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
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
            302,
            DateTime.Today,
            OrderStatus.InProgress,
            [new(product2, 1)],
            DateTime.Today.AddDays(1),
            BranchA, 
            BranchC
        );
        
        order.AssociateWithProduct(new ProductQuantityInOrder(product3, order, 1));

        Assert.Contains(order, product3.AssociatedOrders.Select(p => p.Order));
        Assert.Contains(product3, order.AssociatedProducts.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateWithProductAddsToOrders()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
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
            303,
            DateTime.Today,
            OrderStatus.InProgress,
            [new(product2, 1)],
            DateTime.Today.AddDays(1),
            BranchA, 
            BranchC
        );
        
        product3.AssociateWithOrder(new ProductQuantityInOrder(product3, order, 1));

        Assert.Contains(order, product3.AssociatedOrders.Select(p => p.Order));
        Assert.Contains(product3, order.AssociatedProducts.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateWithOrderThrows()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
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
            302,
            DateTime.Today,
            OrderStatus.InProgress,
            [new(product2, 1)],
            DateTime.Today.AddDays(1),
            BranchA, 
            BranchC
        );
        
        Assert.Throws<ValidationException>(() => order.AssociateWithProduct(new ProductQuantityInOrder(product2, order, 1)));
    }
    
    [Fact]
    public void TestAssociateWithProductThrows()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
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
            303,
            DateTime.Today,
            OrderStatus.InProgress,
            [new(product2, 1)],
            DateTime.Today.AddDays(1),
            BranchA, 
            BranchC
        );
        
        Assert.Throws<ValidationException>(() => product3.AssociateWithOrder(new ProductQuantityInOrder(product2, order, 1)));
    }
    
    [Fact]
    public void TestAssociateAddStockAddsProductAndBranch()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        pickupPoint.AddProductStock(product3, 1);

        Assert.Contains(pickupPoint, product3.StockedIn.Select(s => s.Branch));
        Assert.Contains(product3, pickupPoint.Stocks.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateAddStockAddsQuantity()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        pickupPoint.AddProductStock(product3, 1);
        
        pickupPoint.AddProductStock(product3, 1);

        Assert.Contains(pickupPoint, product3.StockedIn.Select(s => s.Branch));
        Assert.Equal(2, pickupPoint.Stocks.Single(s => s.Product == product3).Quantity);
        Assert.Contains(product3, pickupPoint.Stocks.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateWithProductAddsToBranches()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        product3.AssociateWithBranch(new BranchProductStock(pickupPoint, product3, 1));

        Assert.Contains(pickupPoint, product3.StockedIn.Select(s => s.Branch));
        Assert.Contains(product3, pickupPoint.Stocks.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateWithBranchThrows()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        Assert.Throws<ValidationException>(() => product2.AssociateWithBranch(new BranchProductStock(pickupPoint, product3, 1)));
    }
    
    [Fact]
    public void TestAssociateWithBranchAddsToProducts()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        pickupPoint.AssociateWithProduct(new BranchProductStock(pickupPoint, product3, 1));

        Assert.Contains(pickupPoint, product3.StockedIn.Select(s => s.Branch));
        Assert.Contains(product3, pickupPoint.Stocks.Select(p => p.Product));
    }
    
    [Fact]
    public void TestAssociateWithProductInBranchThrows()
    {
        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(15m, 15m, 15m)
        );
        
        var pickupPoint = new PickupPoint(
            new Address("Street1", "10/2", "app1", "01-234", "City1"),
            "PickupPoint1",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        var pickupPointA = new PickupPoint(
            new Address("Street2", "10/2", "app1", "01-234", "City1"),
            "PickupPoint2",
            "10:00-22:00",
            100m,
            50,
            10m
        );
        
        Assert.Throws<ValidationException>(() => pickupPointA.AssociateWithProduct(new BranchProductStock(pickupPoint, product3, 1)));
    }
}