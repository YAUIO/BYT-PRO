using System.Runtime.Serialization;
using Newtonsoft.Json;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

public abstract class Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static readonly List<Branch> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Branch> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly Address _address;
    private readonly string _name;
    private string _openingHours;
    private readonly decimal _totalArea;
    private readonly List<ProductEntry> _stockCart;


    // ----------< Properties with validation >----------
    public Address Address
    {
        get => _address;
        init
        {
            value.IsNotNull(nameof(Address));
            _address = value;
        }
    }

    public string Name
    {
        get => _name;
        init
        {
            value.IsNotNullOrEmpty(nameof(Name));
            value.IsBelowMaxLength(50);
            _name = value;
        }
    }

    public string OpeningHours
    {
        get => _openingHours;
        set
        {
            value.IsNotNullOrEmpty(nameof(OpeningHours));
            value.IsBelowMaxLength(100);
            _openingHours = value;
        }
    }

    public decimal TotalArea
    {
        get => _totalArea;
        init
        {
            value.IsPositive(nameof(TotalArea));
            _totalArea = value;
        }
    }

    public List<ProductEntry> StockCart
    {
        get => [.._stockCart];
        init
        {
            // 1. Check for nullability
            value.IsNotNull(nameof(value));

            // 2. Check for cart-items nullability and positive quantity
            foreach (var cartItem in value)
            {
                cartItem.IsNotNull(nameof(cartItem));
                cartItem.Product.IsNotNull(nameof(cartItem.Product));
                cartItem.Quantity.IsNotNull(nameof(cartItem.Quantity));
                cartItem.Quantity.IsNonNegative(nameof(cartItem.Quantity));
            }

            // 3. Check for uniqueness of products in the cart
            var duplicates = value
                .GroupBy(cartItem => cartItem.Product)
                .Where(group => group.Count() > 1)
                .ToList();

            if (duplicates.Count > 0)
                throw new ValidationException("Duplicate products in StockCart.");

            _stockCart = value;
        }
    }


    // ----------< Constructor >----------
    protected Branch(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        List<ProductEntry>? stockCart = null)
    {
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        TotalArea = totalArea;
        StockCart = stockCart ?? [];
    }

    // -----< Post Construct >-----
    protected void FinishConstruction()
    {
        // child-specifics hook
        OnAfterConstruction();

        // only parent-specifics
        Extent.Add(this);
    }

    protected virtual void OnAfterConstruction()
    {
    }


    // ----------< Associations >----------

    // -----< Composition >-----
    private readonly HashSet<LocalEmployee> _employees = [];

    [JsonIgnore] public HashSet<LocalEmployee> Employees => [.._employees];

    public void Delete()
    {
        if (_stocks.Count > 0)
            throw new InvalidOperationException("Redistribute stocks before deleting a branch.");

        foreach (var employee in _employees.ToList())
            employee.Delete();

        _employees.Clear();

        Extent.Remove(this);
    }

    public void AddEmployee(LocalEmployee employee)
    {
        employee.IsNotNull(nameof(employee));
        _employees.Add(employee);
    }

    public void RemoveEmployee(LocalEmployee employee)
    {
        employee.IsNotNull(nameof(employee));
        _employees.Remove(employee);
    }

    // -----< Aggregation >-----
    private readonly HashSet<BranchProductStock> _stocks = [];

    [JsonIgnore] public HashSet<BranchProductStock> Stocks => [.._stocks];

    public void AddProductStock(Product product, int quantity)
    {
        var currentStock = GetStock(product);
        if (currentStock != null)
        {
            // Case 1: Product exists in stock
            quantity.IsPositive(nameof(quantity));
            currentStock.Quantity += quantity;
        }
        else
        {
            // Case 2: Product does not exist in stock
            var stock = new BranchProductStock(this, product, quantity);
            _stocks.Add(stock);
            product.AddStock(stock);
        }
    }

    private void ReduceProductStock(Product product, int quantity)
    {
        var currentStock = GetStock(product);
        if (currentStock == null)
            throw new InvalidOperationException($"Product '{product.Name}' is not in stock.");

        quantity.IsPositive(nameof(quantity));

        if (currentStock.Quantity < quantity)
            throw new InvalidOperationException($"Product '{product.Name}' has insufficient stock.");

        currentStock.Quantity -= quantity;
    }

    private BranchProductStock? GetStock(Product product)
    {
        product.IsNotNull(nameof(product));
        return _stocks.FirstOrDefault(stock => stock.Product == product);
    }

    public void EnsureStockForItems(ICollection<ProductEntry> items)
    {
        items.IsNotNull(nameof(items));
        foreach (var item in items)
        {
            var stock = GetStock(item.Product);

            if (stock == null || stock.Quantity < item.Quantity)
                throw new ValidationException(
                    $"Store '{Name}' has insufficient stock for '{item.Product.Name}'. " +
                    $"Requested: {item.Quantity}, Available: {stock?.Quantity ?? 0}");
        }
    }

    public void ReduceStockForItems(ICollection<ProductEntry> items)
    {
        items.IsNotNull(nameof(items));
        foreach (var item in items)
            ReduceProductStock(item.Product, item.Quantity);
    }


    // ----------< JSON >----------
    [OnSerializing]
    private void OnSerializing(StreamingContext context)
    {
        // _stockCart = JSON (de)serializable representation of _stocks
        // _stocks = runtime representation of association between branch and products

        // thus, before persisting, we update _stockCart to reflect the current state of _stocks

        var productEntries = _stocks
            .Select(stock => new ProductEntry(stock.Product, stock.Quantity))
            .ToList();

        _stockCart.Clear();
        _stockCart.AddRange(productEntries);
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        // _stockCart = JSON (de)serializable representation of _stocks
        // _stocks = runtime representation of association between branch and products

        // thus, after this object is deserialized, we must restore _stocks to reflect the saved _stockCart

        foreach (var item in _stockCart)
            AddProductStock(item.Product, item.Quantity);
    }
}