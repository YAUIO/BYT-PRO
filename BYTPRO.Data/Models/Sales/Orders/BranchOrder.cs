using System.Runtime.Serialization;
using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales.Orders;

public class BranchOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static HashSet<BranchOrder> Extent
    {
        get => JsonContext.Context.GetTable<BranchOrder>();
    }

    [JsonIgnore] public new static IReadOnlyList<BranchOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private DateTime _expectedDeliveryDate;


    // ----------< Properties with validation >----------
    public DateTime ExpectedDeliveryDate
    {
        get => _expectedDeliveryDate;
        set
        {
            value.IsNotDefault(nameof(ExpectedDeliveryDate));
            CreationDate.IsBefore(value, nameof(CreationDate), nameof(ExpectedDeliveryDate));
            _expectedDeliveryDate = value;
        }
    }

    public override decimal TotalPrice => 0m;


    // ----------< Constructor >----------
    public BranchOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        DateTime expectedDeliveryDate
    ) : base(id, creationDate, status, cart)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;

        // 1. Associations
        Associate();

        // 2. Extents (parent, child)
        RegisterOrder();
        Extent.Add(this);
    }
}