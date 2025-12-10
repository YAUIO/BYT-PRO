using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation;
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
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        DateTime expectedDeliveryDate,
        Warehouse from,
        Branch to
    ) : base(id, creationDate, status, cart)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
        From = from;
        To = to;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    private readonly Warehouse _from;

    public Warehouse From
    {
        get => _from;
        init
        {
            value.IsNotNull(nameof(From));
            _from = value;
        }
    }

    private readonly Branch _to;

    public Branch To
    {
        get => _to;
        init
        {
            value.IsNotNull(nameof(To));
            if (value is not Store && value is not PickupPoint)
                throw new ValidationException($"{nameof(To)} must be a Store or a PickupPoint.");

            _to = value;
        }
    }

    #endregion
}