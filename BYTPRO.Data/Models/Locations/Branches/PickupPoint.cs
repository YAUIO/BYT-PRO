using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class PickupPoint : Branch
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<PickupPoint> Extent => JsonContext.Context.GetTable<PickupPoint>();

    [JsonIgnore] public new static IReadOnlyList<PickupPoint> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private int _parcelStorageSlots;
    private decimal _maxParcelWeight;

    #endregion

    #region ----------< Properties with validation >----------

    public int ParcelStorageSlots
    {
        get => _parcelStorageSlots;
        set
        {
            value.IsNonNegative(nameof(ParcelStorageSlots));
            _parcelStorageSlots = value;
        }
    }

    public decimal MaxParcelWeight
    {
        get => _maxParcelWeight;
        set
        {
            value.IsPositive(nameof(MaxParcelWeight));
            _maxParcelWeight = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    public PickupPoint(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        int parcelStorageSlots,
        decimal maxParcelWeight)
        : base(address, name, openingHours, totalArea)
    {
        ParcelStorageSlots = parcelStorageSlots;
        MaxParcelWeight = maxParcelWeight;

        FinishConstruction();
    }
    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------
    
    private readonly HashSet<OnlineOrder> _onlineOrders = [];

    [JsonIgnore] public HashSet<OnlineOrder> OnlineOrders => [.._onlineOrders];

    internal void AddOrder(OnlineOrder order)
    {
        order.IsNotNull(nameof(order));
        if (order.PickupPoint != this)
            throw new ValidationException($"{nameof(order.PickupPoint)} must reference this Pickup Point instance.");
        
        _onlineOrders.Add(order);
        
    }
    protected override void OnBranchClose()
    {
        Extent.Remove(this);
    }

    #endregion
}