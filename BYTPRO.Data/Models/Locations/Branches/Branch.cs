using System.Text.Json.Serialization;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

public abstract class Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static readonly List<Branch> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Branch> All => Extent.ToList().AsReadOnly();

    protected void RegisterBranch() => Extent.Add(this);


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

    // ----------< Associations >----------
    private readonly HashSet<LocalEmployee> _employees = [];

    [JsonIgnore] public HashSet<LocalEmployee> Employees => [.._employees];

    public void Delete()
    {
        foreach (var employee in _employees)
        {
            employee.Delete();
        }

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
}