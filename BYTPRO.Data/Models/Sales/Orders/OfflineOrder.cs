using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json;

namespace BYTPRO.Data.Models.Sales.Orders;

public class OfflineOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static HashSet<OfflineOrder> Extent => JsonContext.Context.GetTable<OfflineOrder>();

    [JsonIgnore] public new static IReadOnlyList<OfflineOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly string? _phone;


    // ----------< Properties with validation >----------
    public string? Phone
    {
        get => _phone;
        init
        {
            value?.IsPhoneNumber();
            _phone = value;
        }
    }


    // ----------< Constructor >----------
    public OfflineOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        string? phone,
        Store store
    ) : base(id, creationDate, status, cart)
    {
        Phone = phone;
        Store = store;

        store.EnsureStockForItems(cart);

        // At this point everything is validated, and we enter a post-construction step:
        // 1. Finalize the order by reducing the stocks in the store.
        // 2. Establish reverse connections with: Store, Products.
        // 3. Register the order in all Class Extents. 

        store.ReduceStockForItems(cart);

        Store.AddOrder(this);

        // 1. Associations
        Associate();

        // 2. Extents (parent, child)
        RegisterOrder();
        Extent.Add(this);
    }


    // ----------< Associations >----------
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
}