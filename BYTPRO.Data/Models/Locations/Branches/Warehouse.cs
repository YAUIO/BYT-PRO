using Newtonsoft.Json;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class Warehouse : Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static HashSet<Warehouse> Extent => JsonContext.Context.GetTable<Warehouse>();

    [JsonIgnore] public new static IReadOnlyList<Warehouse> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly decimal _maxStorageCapacity;
    private int _dockCount;
    private decimal _currentStorageCapacity;


    // ----------< Properties with validation >----------
    public decimal MaxStorageCapacity
    {
        get => _maxStorageCapacity;
        init
        {
            value.IsPositive(nameof(MaxStorageCapacity));
            _maxStorageCapacity = value;
        }
    }

    public int DockCount
    {
        get => _dockCount;
        set
        {
            value.IsNonNegative(nameof(DockCount));
            _dockCount = value;
        }
    }

    public decimal CurrentStorageCapacity
    {
        get => _currentStorageCapacity;
        set
        {
            value.IsNonNegative(nameof(CurrentStorageCapacity));

            if (value > MaxStorageCapacity)
                throw new ValidationException(
                    $"{nameof(CurrentStorageCapacity)} cannot exceed {nameof(MaxStorageCapacity)}.");

            _currentStorageCapacity = value;
        }
    }


    // ----------< Constructor >----------
    public Warehouse(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        decimal maxStorageCapacity,
        int dockCount)
        : base(address, name, openingHours, totalArea)
    {
        MaxStorageCapacity = maxStorageCapacity;
        DockCount = dockCount;
        CurrentStorageCapacity = 0;

        FinishConstruction();
    }

    // -----< Post Construct >-----
    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }


    // ----------< Associations >----------
    public new void Delete()
    {
        base.Delete();
        Extent.Remove(this);
    }
}