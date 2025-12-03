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
        Store.AddOrder(this);
        AddItemsToProduct();

        foreach (var item in OrderItems)
        {
            var stock = store.Stocks
                .SingleOrDefault(s => s.Product.Name == item.Product.Name);

            if (stock == null)
            {
                throw new ValidationException($"Store does not have products of type {item.Product.Name}");
            }

            if (stock.Quantity < item.Quantity)
            {
                throw new ValidationException($"Store has less products of type {item.Product.Name} then needed");
            }

            store.Stocks.Remove(stock);

            stock.Quantity -= item.Quantity;

            store.Stocks.Add(stock);
        }

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
        Store.AddOrder(this);

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