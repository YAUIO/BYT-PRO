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
    
    private readonly Branch _from;
    private readonly Branch _to;
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
    
    public Branch From
    {
        get => _from;
        init
        {
            value.IsNotNull(nameof(From));
 
            if (_to is not null && value == _to)
                throw new ValidationException($"{nameof(From)} cannot be the same branch as {nameof(To)}.");

            _from = value;
        }
    }

    public Branch To
    {
        get => _to;
        init
        {
            value.IsNotNull(nameof(To));

            if (_from is not null && value == _from)
                throw new ValidationException($"{nameof(To)} cannot be the same as {nameof(From)}.");

            _to = value;
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
        Branch from,
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
}