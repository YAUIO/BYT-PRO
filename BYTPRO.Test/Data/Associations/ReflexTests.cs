using BYTPRO.Data.Models.Sales;

namespace BYTPRO.Test.Data.Associations;

public class ReflexTests
{
    [Fact]
    public void TestCreateProductOfOtherProducts()
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

        var product3ConsistsOf = product3.ConsistsOf;

        Assert.Contains(product1, product3ConsistsOf);
        Assert.Contains(product2, product3ConsistsOf);
    }
}