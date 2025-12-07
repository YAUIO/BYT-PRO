using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales.Orders;

public class BranchOrder : Order
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<BranchOrder> Extent => JsonContext.Context.GetTable<BranchOrder>();

    [JsonIgnore] public new static IReadOnlyList<BranchOrder> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private DateTime _expectedDeliveryDate;

    #endregion

    #region ----------< Properties with validation >----------

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

    #endregion

    #region ----------< Construction >----------

    public BranchOrder(
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        DateTime expectedDeliveryDate
    ) : base(creationDate, status, cart)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion
}