using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;

namespace BYTPRO.Test.Data.Associations;

public class BasicTests
{
    [Fact]
    public void TestCreate()
    {
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );

        var customer = new Customer(
            102,
            "Artiom",
            "Bezkorovainyi",
            "+48000000001",
            "s30001@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);

        var offlineOrder = new OfflineOrder(
            101,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product1, 2), new ProductEntry(product2, 2)],
            null,
            store
        );
    }

    [Fact]
    public void TestCreateChangesStock()
    {
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );

        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);

        var offlineOrder = new OfflineOrder(
            102,
            DateTime.Now,
            OrderStatus.InProgress,
            [new ProductEntry(product1, 2), new ProductEntry(product2, 2)],
            null,
            store
        );

        Assert.Equal(3, store.Stocks.Single(s => s.Product.Name.Equals(product1.Name)).Quantity);
        Assert.Equal(8, store.Stocks.Single(s => s.Product.Name.Equals(product2.Name)).Quantity);
    }

    [Fact]
    public void TestCreateFailsIfNoProductIsPresent()
    {
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );

        var customer = new Customer(
            4,
            "Artiom",
            "Bezkorovainyi",
            "+48000000003",
            "s30003@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);

        var product3 = new Product(
            "Product3",
            "Description3",
            50m,
            ["/Product3_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );

        Assert.Throws<ValidationException>(() =>
        {
            var offlineOrder = new OfflineOrder(
                103,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product1, 2), new ProductEntry(product3, 2)],
                null,
                store
            );
        });
    }

    [Fact]
    public void TestCreateFailsIfNotEnoughQuantityInStock()
    {
        var store = new Store(
            new Address("Street2", "20/2", null, "01-345", "City2"),
            "Store1",
            "09:00-22:00",
            1000m,
            5,
            500m,
            3
        );

        var customer = new Customer(
            6,
            "Artiom",
            "Bezkorovainyi",
            "+48000000004",
            "s30004@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        var product1 = new Product(
            "Product1",
            "Description1",
            100m,
            ["/Product1_1.png", "/Product1_2.png"],
            10m,
            new Dimensions(10m, 10m, 10m)
        );
        store.AddProductStock(product1, 2);
        store.AddProductStock(product1, 3);

        var product2 = new Product(
            "Product2",
            "Description2",
            50m,
            ["/Product2_1.png"],
            5m,
            new Dimensions(5m, 5m, 5m)
        );
        store.AddProductStock(product2, 10);

        Assert.Throws<ValidationException>(() =>
        {
            var offlineOrder = new OfflineOrder(
                104,
                DateTime.Now,
                OrderStatus.InProgress,
                [new ProductEntry(product1, 10), new ProductEntry(product2, 2)],
                null,
                store
            );
        });
    }
}