using System.Linq;
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
    private OrderStatus _status ;
    private List<OrderItem> _orderItems = [];


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

    public List<OrderItem> OrderItems
    {
        get => _orderItems;
        set
        {
            value.IsNotNullOrEmpty(nameof(OrderItems));
            
            if (value.Any(i => i is null))
                throw new ValidationException($"{nameof(OrderItems)} cannot contain null items.");

            _orderItems = value;
        }
    }


    // ----------< Calculated >----------
    public virtual decimal TotalPrice => _orderItems.Sum(item => item.TotalPrice);

    public decimal TotalWeight => _orderItems.Sum(item => item.Product.Weight * item.Quantity);

    public decimal TotalDimensions => _orderItems.Sum(item => item.Product.Dimensions.Volume * item.Quantity);


    // ----------< Constructor >----------
    protected Order(
        int id,
        DateTime creationDate,
        List<OrderItem>? orderItems = null)
    {
        Id = id;
        CreationDate = creationDate;
        Status = OrderStatus.InProgress;
        if (orderItems is not null) OrderItems = orderItems;
    }
}