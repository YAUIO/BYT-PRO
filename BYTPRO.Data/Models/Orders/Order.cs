using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Models.UmlAttributes;

namespace BYTPRO.Data.Models.Orders;

public abstract class Order
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static readonly List<Order> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Order> All => Extent.ToList().AsReadOnly();

    protected void RegisterOrder() => Extent.Add(this);


    // ----------< Attributes >----------
    private readonly int _id;
    private readonly DateTime _creationDate;
    private OrderStatus _status;
    private readonly List<OrderItem> _orderItems = [];


    // ----------< Properties with validation >----------
    public int Id
    {
        get => _id;
        init
        {
            value.IsNonNegative(nameof(Id));
            if (Extent.Any(o => o.Id == value))
                throw new ValidationException($"Order with Id {value} already exists.");
            _id = value;
        }
    }

    public DateTime CreationDate
    {
        get => _creationDate;
        init
        {
            value.IsNotDefault(nameof(CreationDate));
            value.IsBefore(DateTime.UtcNow, nameof(CreationDate), "now");
            _creationDate = value;
        }
    }

    public OrderStatus Status
    {
        get => _status;
        set
        {
            value.IsDefined(nameof(Status));
            _status = value;
        }
    }

    public ReadOnlyCollection<OrderItem> OrderItems
    {
        get => _orderItems.AsReadOnly();
        init
        {
            value.IsNotNullOrEmpty(nameof(OrderItems));
            _orderItems.AddRange(value);
        }
    }


    // ----------< Calculated Properties >----------
    [JsonIgnore] public virtual decimal TotalPrice => _orderItems.Sum(item => item.TotalPrice);

    [JsonIgnore] public decimal TotalWeight => _orderItems.Sum(item => item.TotalWeight);

    [JsonIgnore] public decimal TotalDimensions => _orderItems.Sum(item => item.TotalDimensions);


    // ----------< Constructor >----------
    protected Order(
        int id,
        DateTime creationDate,
        List<OrderItem> orderItems)
    {
        Id = id;
        CreationDate = creationDate;
        Status = OrderStatus.InProgress;
        OrderItems = new ReadOnlyCollection<OrderItem>(orderItems);
    }
}