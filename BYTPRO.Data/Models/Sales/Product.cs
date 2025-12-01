using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales;

public class Product
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<Product> Extent => JsonContext.Context.GetTable<Product>();

    [JsonIgnore] public static IReadOnlyList<Product> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly string _name;
    private string _description;
    private readonly decimal _price;
    private readonly List<string> _images = [];
    private readonly decimal _weight;
    private readonly Dimensions _dimensions;


    // ----------< Properties with validation >----------
    public string Name
    {
        get => _name;
        init
        {
            value.IsNotNullOrEmpty(nameof(Name));
            value.IsBelowMaxLength(100);
            _name = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            value.IsNotNullOrEmpty(nameof(Description));
            value.IsBelowMaxLength(1000);
            _description = value;
        }
    }

    public decimal Price
    {
        get => _price;
        init
        {
            value.IsPositive(nameof(Price));
            _price = value;
        }
    }

    public List<string> Images
    {
        get => _images;
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(Images));
            _images.AddRange(value);
        }
    }

    public decimal Weight
    {
        get => _weight;
        init
        {
            value.IsPositive(nameof(Weight));
            _weight = value;
        }
    }

    public Dimensions Dimensions
    {
        get => _dimensions;
        init
        {
            value.IsNotNull(nameof(Dimensions));
            _dimensions = value;
        }
    }


    // ----------< Constructor >----------
    public Product(
        string name,
        string description,
        decimal price,
        List<string> images,
        decimal weight,
        Dimensions dimensions,
        HashSet<Product>? consistsOf = null
    )
    {
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        Weight = weight;
        Dimensions = dimensions;

        Extent.Add(this);
    }


    // ----------< Associations >----------

    // -----< with attribute >-----
    private readonly HashSet<ProductQuantityInOrder> _orderItems = [];

    [JsonIgnore] public HashSet<ProductQuantityInOrder> OrderItems => [.._orderItems]; // Returns a new copy

    public void AddOrderItem(ProductQuantityInOrder orderItem)
    {
        orderItem.IsNotNull(nameof(orderItem));
        _orderItems.Add(orderItem);
    }
}