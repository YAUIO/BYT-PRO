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
    
    private readonly string _from;
    private readonly string _to;

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
    
    public string From
    {
        get => _from;
        init
        {
            value.IsNotNullOrEmpty(nameof(From));
            value.IsBelowMaxLength(50);
 
            if (_to is not null && value == _to)
                throw new ValidationException($"{nameof(From)} cannot be the same as {nameof(To)}.");

            _from = value;
        }
    }

    public string To
    {
        get => _to;
        init
        {
            value.IsNotNullOrEmpty(nameof(To));
            value.IsBelowMaxLength(50);

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
        string from,
        string to
    ) : base(id, creationDate, status, cart)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
        From = from;
        To = to;
        
        FinishConstruction();
    }
    
    [JsonConstructor]
    private BranchOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        DateTime expectedDeliveryDate,
        string from,
        string to,
        bool fromJson = true  
    ) : base(id, creationDate, status, cart)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
        
        _from = from;
        _to = to;
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion
}