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
    [JsonIgnore] private static JsonEntitySet<OnlineOrder> Extent => JsonContext.Context.GetTable<OnlineOrder>();

    [JsonIgnore] public new static IReadOnlyList<OnlineOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private bool _isPaid;
    private DateTime? _cancellationDate;
    private readonly string _trackingNumber;


    // ----------< Properties with validation >----------
    #pragma warning disable S2292
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
    #pragma warning restore S2292

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
        Dictionary<Product, int> orderItems,
        bool isPaid,
        string trackingNumber,
        Customer customer
    ) : base(id, creationDate, orderItems)
    {
        IsPaid = isPaid;
        TrackingNumber = trackingNumber;

        Customer = customer;
        customer.AddOrder(this);
        AddItemsToProduct();

        RegisterOrder();
        Extent.Add(this);
    }
    
    [JsonConstructor]
    private OnlineOrder(
        int id,
        DateTime creationDate,
        HashSet<ProductQuantityInOrder> orderItems,
        bool isPaid,
        string trackingNumber,
        Customer customer
    ) : base(id, creationDate, orderItems)
    {
        IsPaid = isPaid;
        TrackingNumber = trackingNumber;

        Customer = customer;
    }
    
    [OnDeserialized]
    internal void Register(StreamingContext context)
    {
        if (Extent.Any(c => c.Id == Id))
            return;

        Extent.Add(this);
        AddItemsToProduct();
        RegisterOrder();
        Customer.AddOrder(this);
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