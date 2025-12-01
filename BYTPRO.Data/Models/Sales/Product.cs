using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Locations.Branches;
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
    private readonly DeserializableReadOnlyList<string> _images;
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

    public DeserializableReadOnlyList<string> Images
    {
        get => _images;
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(Images));
            _images = value;
            _images.MakeReadOnly();
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
        DeserializableReadOnlyList<string> images,
        decimal weight,
        Dimensions dimensions,
        Dictionary<Branch, int> initialStock,
        HashSet<Product>? consistsOf = null
    )
    {
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        Weight = weight;
        Dimensions = dimensions;

        initialStock.IsNotNullOrEmpty(nameof(initialStock));
        InitializeStock(initialStock);

        ConsistsOf = consistsOf;

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

    // -----< Reflex >-----
    private readonly HashSet<Product>? _consistsOf;

    [JsonIgnore]
    public HashSet<Product>? ConsistsOf
    {
        get => _consistsOf == null ? null : [.._consistsOf];
        init
        {
            value?.AreAllElementsNotNull(nameof(ConsistsOf));
            _consistsOf = value;
        }
    }

    // -----< Aggregation (In Branches) >-----
    private readonly HashSet<BranchProductStock> _stockedIn = [];

    [JsonIgnore] public HashSet<BranchProductStock> StockedIn => [.._stockedIn];

    public void AddStock(BranchProductStock stock)
    {
        stock.IsNotNull(nameof(stock));
        _stockedIn.Add(stock);
    }

    public void RemoveStock(BranchProductStock stock)
    {
        stock.IsNotNull(nameof(stock));
        _stockedIn.Remove(stock);
    }

    private void InitializeStock(Dictionary<Branch, int> initialStock)
    {
        foreach (var item in initialStock)
        {
            var branch = item.Key;
            var quantity = item.Value;

            branch.IsNotNull(nameof(branch));
            quantity.IsNonNegative(nameof(quantity));

            _ = new BranchProductStock(branch, this, quantity);
        }
    }
}