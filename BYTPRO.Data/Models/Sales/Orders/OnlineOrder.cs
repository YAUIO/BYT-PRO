using BYTPRO.Data.Models.Locations.Branches;
using Newtonsoft.Json;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales.Orders;

public class OnlineOrder : Order
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<OnlineOrder> Extent => JsonContext.Context.GetTable<OnlineOrder>();

    [JsonIgnore] public new static IReadOnlyList<OnlineOrder> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private bool _isPaid;
    private DateTime? _cancellationDate;
    private readonly string _trackingNumber;

    #endregion

    #region ----------< Properties with validation >----------

    public bool IsPaid
    {
        get => _isPaid;
        set
        {
            // Do not convert into auto-property.
            // Maybe some business rule will be added here
            _isPaid = value;
        }
    }

    public DateTime? CancellationDate
    {
        get => _cancellationDate;
        set
        {
            if (value is null) return; // During deserialization this setter is called even with null value

            if (Status != OrderStatus.AwaitingCollection)
                throw new ValidationException(
                    "Cancellation date can only be set in 'Awaiting Collection' status.");

            value?.IsNotDefault(nameof(CancellationDate));
            value?.IsAfter(CreationDate, nameof(CancellationDate), nameof(CreationDate));
            _cancellationDate = value;
        }
    }

    public string TrackingNumber
    {
        get => _trackingNumber;
        init
        {
            value.IsNotNullOrEmpty(nameof(TrackingNumber));
            value.IsBelowMaxLength(50);

            if (Extent.Any(o => o.TrackingNumber == value))
                throw new ValidationException($"OnlineOrder with TrackingNumber {value} already exists.");

            _trackingNumber = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    public OnlineOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        bool isPaid,
        DateTime? cancellationDate,
        string trackingNumber,
        Customer customer,
        PickupPoint pickupPoint
    ) : base(id, creationDate, status, cart)
    {
        IsPaid = isPaid;
        CancellationDate = cancellationDate;
        TrackingNumber = trackingNumber;
        Customer = customer;
        PickupPoint = pickupPoint;

        FinishConstruction();
    }
    
    [JsonConstructor]
    private OnlineOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        bool isPaid,
        DateTime? cancellationDate,
        string trackingNumber,
        Customer customer,
        PickupPoint pickupPoint,
        bool fromJson = true
    ) : base(id, creationDate, status, cart)
    {
        IsPaid = isPaid;
        CancellationDate = cancellationDate;
        _trackingNumber = trackingNumber;
        Customer = customer;
        PickupPoint = pickupPoint; 
    }

    protected override void OnAfterConstruction()
    {
        Customer.AddOrder(this);
        PickupPoint.AddOrder(this);
        
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    private readonly Customer _customer;
    private readonly PickupPoint _pickupPoint;

    public Customer Customer
    {
        get => _customer;
        init
        {
            value.IsNotNull(nameof(Customer));
            _customer = value;
        }
    }
    
    public PickupPoint PickupPoint
    {
        get => _pickupPoint;
        init
        {
            value.IsNotNull(nameof(PickupPoint));
            _pickupPoint = value;
        }
    }
    #endregion
}