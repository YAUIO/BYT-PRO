using System.Runtime.Serialization;
using Newtonsoft.Json;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Sales.Orders;

public class OnlineOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static HashSet<OnlineOrder> Extent => JsonContext.Context.GetTable<OnlineOrder>();

    [JsonIgnore] public new static IReadOnlyList<OnlineOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private bool _isPaid;
    private DateTime? _cancellationDate;
    private readonly string _trackingNumber;


    // ----------< Properties with validation >----------
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
            _trackingNumber = value;
        }
    }


    // ----------< Constructor >----------
    public OnlineOrder(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart,
        bool isPaid,
        string trackingNumber,
        Customer customer
    ) : base(id, creationDate, status, cart)
    {
        IsPaid = isPaid;
        TrackingNumber = trackingNumber;
        Customer = customer;

        // 1. Associations
        Customer.AddOrder(this);
        Associate();

        // 2. Extents (parent, child)
        RegisterOrder();
        Extent.Add(this);
    }


    // ----------< Associations >----------
    private readonly Customer _customer;

    public Customer Customer
    {
        get => _customer;
        init
        {
            value.IsNotNull(nameof(Customer));
            _customer = value;
        }
    }
}