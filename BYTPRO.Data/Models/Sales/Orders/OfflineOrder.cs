using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json;

namespace BYTPRO.Data.Models.Sales.Orders;

public class OfflineOrder : Order
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<OfflineOrder> Extent => JsonContext.Context.GetTable<OfflineOrder>();

    [JsonIgnore] public new static IReadOnlyList<OfflineOrder> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly string? _phone;

    #endregion

    #region ----------< Properties with validation >----------

    public string? Phone
    {
        get => _phone;
        init
        {
            value?.IsPhoneNumber();
            _phone = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    public OfflineOrder(
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        string? phone,
        Store store
    ) : base(creationDate, status, cart)
    {
        Phone = phone;
        Store = store;
        Store.EnsureStockForItems(cart);

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        Store.ReduceStockForItems(Cart); // TODO: this should not be called by JSON on deserialization
        Store.AddOrder(this);

        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    private readonly Store _store;

    public Store Store
    {
        get => _store;
        init
        {
            value.IsNotNull(nameof(Store));
            _store = value;
        }
    }

    #endregion
}