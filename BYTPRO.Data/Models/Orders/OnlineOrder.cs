using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Branches;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Orders;

public class OnlineOrder : Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<OnlineOrder> Extent => JsonContext.Context.GetTable<OnlineOrder>();
    [JsonIgnore] public new static IReadOnlyList<OnlineOrder> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private bool _isPaid;
    private DateTime? _cancellationDate;
    private string _trackingNumber = null!;
    private PickupPoint _destinationPickupPoint = null!;
    private readonly Customer _customer = null!;


    // ----------< Properties with validation >----------
    public bool IsPaid
    {
        get => _isPaid;
        set => _isPaid = value;
    }

    public DateTime? CancellationDate
    {
        get => _cancellationDate;
        private set
        {
            if (value.HasValue)
            {
                value.Value.IsNotDefault(nameof(CancellationDate));
                CreationDate.IsBefore(value.Value, nameof(CreationDate), nameof(CancellationDate));
            }
            _cancellationDate = value;
        }
    }

    public string TrackingNumber
    {
        get => _trackingNumber;
        set
        {
            value.IsNotNullOrEmpty(nameof(TrackingNumber));
            value.IsBelowMaxLength(50);
            _trackingNumber = value;
        }
    }

    public PickupPoint DestinationPickupPoint
    {
        get => _destinationPickupPoint;
        set
        {
            value.IsNotNull(nameof(DestinationPickupPoint));
            _destinationPickupPoint = value;
        }
    }

    public Customer Customer
    {
        get => _customer;
        init
        {
            value.IsNotNull(nameof(Customer));
            _customer = value;
        }
    }


    // ----------< Constructor >----------
    public OnlineOrder(
        int id,
        DateTime creationDate,
        Customer customer,
        string trackingNumber,
        PickupPoint destinationPickupPoint
    ) : base(id, creationDate)
    {
        Customer = customer;
        TrackingNumber = trackingNumber;
        DestinationPickupPoint = destinationPickupPoint;
        
        RegisterOrder();
        Extent.Add(this);
    }
}