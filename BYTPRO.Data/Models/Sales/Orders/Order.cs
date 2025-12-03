using Newtonsoft.Json;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.Sales.Orders;

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
            value.IsBefore(DateTime.Now, nameof(CreationDate), "Now");
            _creationDate = value;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public OrderStatus Status
    {
        get => _status;
        set
        {
            value.IsDefined(nameof(Status));
            _status = value;
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
        Dictionary<Product, int> orderItems)
    {
        Id = id;
        CreationDate = creationDate;
        Status = OrderStatus.InProgress;
        _orderItems = InitializeProductQuantities(orderItems);
    }
    
    protected Order(
        int id,
        DateTime creationDate,
        HashSet<ProductQuantityInOrder> orderItems)
    {
        Id = id;
        CreationDate = creationDate;
        Status = OrderStatus.InProgress;
        _orderItems = orderItems;
    }


    // ----------< Associations >----------
    private readonly HashSet<ProductQuantityInOrder> _orderItems;

    public HashSet<ProductQuantityInOrder> OrderItems => [.._orderItems]; // Returns a new copy


    // ----------< Association Methods >----------
    private HashSet<ProductQuantityInOrder> InitializeProductQuantities(
        Dictionary<Product, int> orderItems)
    {
        orderItems.IsNotNull(nameof(orderItems));
        orderItems.Count.IsPositive(nameof(orderItems));
        return orderItems
            .Select(e => new ProductQuantityInOrder(e.Key, this, e.Value))
            .ToHashSet();
    }

    protected void AddItemsToProduct()
    {
        foreach (var item in OrderItems)
        {
            if (item.Product != null)
                item.Product.AddOrderItem(item);
        }
    }
}