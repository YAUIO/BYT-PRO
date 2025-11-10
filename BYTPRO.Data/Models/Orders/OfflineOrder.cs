using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Branches;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Orders;

public class OfflineOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<OfflineOrder> Extent => JsonContext.Context.GetTable<OfflineOrder>();
    [JsonIgnore] public new static IReadOnlyList<OfflineOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private string? _phone;
    private Store _originStore = null!;


    // ----------< Properties with validation >----------
    public string? Phone
    {
        get => _phone;
        set
        {
            if (value is not null)
                value.IsPhoneNumber(nameof(Phone));
            _phone = value;
        }
    }

    public Store OriginStore
    {
        get => _originStore;
        set
        {
            value.IsNotNull(nameof(OriginStore));
            _originStore = value;
        }
    }


    // ----------< Constructor >----------
    public OfflineOrder(
        int id,
        DateTime creationDate,
        Store originStore,
        string? phone = null
    ) : base(id, creationDate)
    {
        OriginStore = originStore;
        Phone = phone;
        
        RegisterOrder();
        Extent.Add(this);
    }
}