using BYTPRO.Data.Models.Sales;

namespace BYTPRO.Test.Data.Associations;

public class ReflexTests
{
    [Fact]
    public void TestCreateProductOfOtherProducts()
    {
        var cpu = new Product(
            "CPU",
            "Best Central Processor ever",
            10000m,
            ["/CPU1.png", "/CPU2.png"],
            0.1m,
            new Dimensions(0.1m, 0.1m, 0.1m)
        );

        var gpu = new Product(
            "GPU",
            "Best Graphics Card ever",
            20000m,
            ["/GPU1.png", "/GPU2.png"],
            1m,
            new Dimensions(0.5m, 0.25m, 0.1m)
        );

        var motherboard = new Product(
            "Motherboard",
            "Best Motherboard ever",
            5000m,
            ["/Motherboard.png"],
            0.5m,
            new Dimensions(0.5m, 0.5m, 0.2m)
        );

        var computer = new Product(
            "Computer",
            "Best Computer ever",
            cpu.Price + gpu.Price + motherboard.Price + 2000m,
            ["/Computer1.png", "/Computer2.png"],
            3m,
            new Dimensions(1m, 1m, 1m),
            [cpu, gpu, motherboard]
        );

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