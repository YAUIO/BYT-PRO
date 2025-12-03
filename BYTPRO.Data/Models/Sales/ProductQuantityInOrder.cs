using Newtonsoft.Json;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record ProductQuantityInOrder
{
    private readonly Product _product;
    private readonly Order _order;
    private readonly int _quantity;

    // ----------< Properties >----------
    public Product Product
    {
        get => _product;
        init
        {
            value.IsNotNull(nameof(Product));
            _product = value;
        }
    }

    public Order Order {
        get => _order;
        init
        {
            value.IsNotNull(nameof(Product));
            _order = value;
        }
    }

    public int Quantity {
        get => _quantity;
        init
        {
            value.IsNotNull(nameof(Product));
            _quantity = value;
        }
    }


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
        Product = product;
        Order = order;
        Quantity = quantity;
    }
    
    [JsonConstructor]
    private ProductQuantityInOrder() {}
}