using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

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
        string? phone
    ) : base(id, creationDate, orderItems)
    {
        Phone = phone;

        AddItemsToProduct();

        RegisterOrder();
        Extent.Add(this);
    }
}