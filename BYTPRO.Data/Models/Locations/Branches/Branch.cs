using Newtonsoft.Json;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

public abstract class Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static readonly List<Branch> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Branch> All => Extent.ToList().AsReadOnly();

    protected void RegisterBranch()
    {
        if (Extent.All(b => b.Name != Name))
            Extent.Add(this);
    }


    // ----------< Attributes >----------
    private readonly Address _address;
    private readonly string _name;
    private string _openingHours;
    private readonly decimal _totalArea;


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


    // ----------< Constructor >----------
    protected Branch(
        Address address,
        string name,
        string openingHours,
        decimal totalArea)
    {
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        TotalArea = totalArea;
    }

    [JsonConstructor]
    protected Branch() {}


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

    public void AddProductStock(
        Product product,
        int quantity)
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

    private BranchProductStock? GetStock(Product product)
    {
        product.IsNotNull(nameof(product));
        return _stocks.FirstOrDefault(stock => stock.Product == product);
    }
}