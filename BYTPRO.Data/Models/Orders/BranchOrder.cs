using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Orders;

public class BranchOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<BranchOrder> Extent => JsonContext.Context.GetTable<BranchOrder>();
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
        DateTime expectedDeliveryDate
    ) : base(id, creationDate)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
        
        RegisterOrder();
        Extent.Add(this);
    }
}