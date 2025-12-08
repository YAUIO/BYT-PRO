using Newtonsoft.Json;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales;

public class Product
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<Product> Extent => JsonContext.Context.GetTable<Product>();

    [JsonIgnore] public static IReadOnlyList<Product> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly string _name;
    private string _description;
    private readonly decimal _price;
    private readonly DeserializableReadOnlyList<string> _images;
    private readonly decimal _weight;
    private readonly Dimensions _dimensions;

    #endregion

    #region ----------< Properties with validation >----------

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

    #endregion

    #region ----------< Constructor >----------

    public Product(
        string name,
        string description,
        decimal price,
        DeserializableReadOnlyList<string> images,
        decimal weight,
        Dimensions dimensions,
        HashSet<Product>? consistsOf = null)
    {
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        Weight = weight;
        Dimensions = dimensions;
        ConsistsOf = consistsOf ?? [];

        FinishConstruction();
    }

    private void FinishConstruction()
    {
        foreach (var product in _consistsOf)
            product._consistsIn.Add(this);

        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    #region -----< with attribute >-----

    private readonly HashSet<ProductQuantityInOrder> _usedInOrders = [];

    [JsonIgnore] public HashSet<ProductQuantityInOrder> AssociatedOrders => [.._usedInOrders];

    public void AssociateWithOrder(ProductQuantityInOrder orderItem)
    {
        orderItem.IsNotNull(nameof(orderItem));
        if (orderItem.Product != this)
            throw new ValidationException($"{nameof(orderItem.Product)} must reference this Product instance.");

        if (_usedInOrders.Add(orderItem))
            orderItem.Order.AssociateWithProduct(orderItem);
    }

    #endregion

    #region -----< Reflex >-----

    private readonly HashSet<Product> _consistsOf;

    public HashSet<Product> ConsistsOf
    {
        get => [.._consistsOf];
        init
        {
            value.IsNotNull(nameof(ConsistsOf));
            value.AreAllElementsNotNull(nameof(ConsistsOf));
            _consistsOf = value;
        }
    }

    // Reverse connection
    private readonly HashSet<Product> _consistsIn = [];

    [JsonIgnore] public HashSet<Product> ConsistsIn => [.._consistsIn];

    #endregion

    #region -----< Aggregation >-----

    private readonly HashSet<BranchProductStock> _stockedIn = [];

    [JsonIgnore] public HashSet<BranchProductStock> StockedIn => [.._stockedIn];

    public void AddStock(BranchProductStock stock)
    {
        stock.IsNotNull(nameof(stock));
        _stockedIn.Add(stock);
    }

    #endregion

    #endregion
}