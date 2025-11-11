using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record OrderItem
{
    // ----------< Properties >----------
    public Product Product { get; }

    public int Quantity { get; }


    // ----------< Calculated Properties >----------
    [JsonIgnore] public decimal TotalPrice => Quantity * Product.Price;

    [JsonIgnore] public decimal TotalWeight => Quantity * Product.Weight;

    [JsonIgnore] public decimal TotalDimensions => Quantity * Product.Dimensions.Volume;


    // ----------< Constructor with validation >----------
    public OrderItem(
        Product product,
        int quantity)
    {
        product.IsNotNull(nameof(Product));
        quantity.IsPositive(nameof(Quantity));

        Product = product;
        Quantity = quantity;
    }
}