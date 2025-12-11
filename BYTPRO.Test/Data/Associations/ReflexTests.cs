using BYTPRO.Data.Models.Sales;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class ReflexTests
{
    [Fact]
    public void TestCreateProductOfOtherProducts()
    {
        var cpu = SalesFactory.CreateProduct();
        var gpu = SalesFactory.CreateProduct();
        var motherboard = SalesFactory.CreateProduct();
        var computer = SalesFactory.CreateProduct(consistsOf: [cpu, gpu, motherboard]);

        // Extents
        var registeredProducts = Product.All;
        Assert.Contains(cpu, registeredProducts);
        Assert.Contains(gpu, registeredProducts);
        Assert.Contains(motherboard, registeredProducts);
        Assert.Contains(computer, registeredProducts);

        // Reflex connection
        Assert.Empty(cpu.ConsistsOf);
        Assert.Empty(gpu.ConsistsOf);
        Assert.Empty(motherboard.ConsistsOf);

        var computerParts = computer.ConsistsOf;
        Assert.Contains(cpu, computerParts);
        Assert.Contains(gpu, computerParts);
        Assert.Contains(motherboard, computerParts);

        // Reverse reflex connection
        Assert.Contains(computer, cpu.ConsistsIn);
        Assert.Contains(computer, gpu.ConsistsIn);
        Assert.Contains(computer, motherboard.ConsistsIn);
    }
}