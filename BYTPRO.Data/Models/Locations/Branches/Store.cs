using Newtonsoft.Json;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class Store : Branch
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<Store> Extent => JsonContext.Context.GetTable<Store>();

    [JsonIgnore] public new static IReadOnlyList<Store> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private int _posCount;
    private decimal _salesArea;
    private int _selfCheckouts;

    #endregion

    #region ----------< Properties with validation >----------

    public int PosCount
    {
        get => _posCount;
        set
        {
            value.IsNonNegative(nameof(PosCount));
            _posCount = value;
        }
    }

    public decimal SalesArea
    {
        get => _salesArea;
        set
        {
            value.IsPositive(nameof(SalesArea));
            if (value > TotalArea)
                throw new ValidationException(
                    $"{nameof(SalesArea)} cannot exceed {nameof(TotalArea)} ({value} > {TotalArea})");
            _salesArea = value;
        }
    }

    public int SelfCheckouts
    {
        get => _selfCheckouts;
        set
        {
            value.IsNonNegative(nameof(SelfCheckouts));
            _selfCheckouts = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    public Store(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        int posCount,
        decimal salesArea,
        int selfCheckouts)
        : base(address, name, openingHours, totalArea)
    {
        PosCount = posCount;
        SalesArea = salesArea;
        SelfCheckouts = selfCheckouts;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    private readonly HashSet<OfflineOrder> _offlineOrders = [];

    [JsonIgnore] public HashSet<OfflineOrder> OfflineOrders => [.._offlineOrders];

    internal void AddOrder(OfflineOrder order)
    {
        order.IsNotNull(nameof(order));
        if (order.Store != this)
            throw new ValidationException($"{nameof(order.Store)} must reference this Store instance.");

        _offlineOrders.Add(order);
    }

    protected override void OnBranchClose()
    {
        Extent.Remove(this);
    }

    #endregion
}