using Newtonsoft.Json;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record ProductQuantityInOrder
{
    // ----------< Properties >----------
    public Product Product { get; }
    public Order Order { get; }
    public int Quantity { get; }


    // ----------< Calculated Properties >----------
    [JsonIgnore] public decimal TotalPrice => Quantity * Product.Price;
    [JsonIgnore] public decimal TotalWeight => Quantity * Product.Weight;
    [JsonIgnore] public decimal TotalDimensions => Quantity * Product.Dimensions.Volume;


    // ----------< Constructor with validation >----------
    public ProductQuantityInOrder(
        Product product,
        Order order,
        int quantity)
    {
        product.IsNotNull(nameof(Product));
        order.IsNotNull(nameof(Order));
        quantity.IsPositive(nameof(Quantity));

        Product = product;
        Order = order;
        Quantity = quantity;
    }
}