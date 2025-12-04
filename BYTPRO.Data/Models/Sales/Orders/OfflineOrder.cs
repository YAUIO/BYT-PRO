using System.Runtime.Serialization;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json;

namespace BYTPRO.Data.Models.Sales.Orders;

public class OfflineOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<OfflineOrder> Extent => JsonContext.Context.GetTable<OfflineOrder>();

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
        Dictionary<Product, int> orderItems,
        string? phone,
        Store store
    ) : base(id, creationDate, orderItems)
    {
        Phone = phone;

        Store = store;

        store.HasAllItemsInStock(OrderItems);

        Store.AddOrder(this);
        AddItemsToProduct();

        RegisterOrder();
        Extent.Add(this);
    }

    [JsonConstructor]
    private OfflineOrder(
        int id,
        DateTime creationDate,
        HashSet<ProductQuantityInOrder> orderItems,
        string? phone,
        Store store
    ) : base(id, creationDate, orderItems)
    {
        Id = id;
        CreationDate = creationDate;
        Phone = phone;
        Store = store;
    }

    [OnDeserialized]
    internal void Register(StreamingContext context)
    {
        if (Extent.Any(c => c.Id == Id))
            return;

        Store.AddOrder(this);

        AddItemsToProduct();
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