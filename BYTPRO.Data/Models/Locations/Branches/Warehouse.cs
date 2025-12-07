using Newtonsoft.Json;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class Warehouse : Branch
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<Warehouse> Extent => JsonContext.Context.GetTable<Warehouse>();

    [JsonIgnore] public new static IReadOnlyList<Warehouse> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly decimal _maxStorageCapacity;
    private int _dockCount;
    private decimal _currentStorageCapacity;

    #endregion

    #region ----------< Properties with validation >----------

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

    #endregion

    #region ----------< Construction >----------

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

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    protected override void OnDelete()
    {
        Extent.Remove(this);
    }

    #endregion
}