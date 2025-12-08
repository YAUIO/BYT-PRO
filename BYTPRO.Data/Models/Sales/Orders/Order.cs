using Newtonsoft.Json;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.Sales.Orders;

public abstract class Order
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static readonly List<Order> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Order> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly int _id;
    private readonly DateTime _creationDate;
    private OrderStatus _status;
    private readonly DeserializableReadOnlyList<ProductEntry> _cart;

    #endregion

    #region ----------< Properties with validation >----------

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

    public DeserializableReadOnlyList<ProductEntry> Cart
    {
        get => _cart;
        init
        {
            // 1. Check for nullability and count
            value.IsNotNull(nameof(Cart));
            value.Count.IsPositive(nameof(Cart));

            // 2. Check for cart-items nullability and positive quantity
            foreach (var cartItem in value)
            {
                cartItem.IsNotNull(nameof(cartItem));
                cartItem.Product.IsNotNull(nameof(cartItem.Product));
                cartItem.Quantity.IsNotNull(nameof(cartItem.Quantity));
                cartItem.Quantity.IsPositive(nameof(cartItem.Quantity));
            }

            // 3. Check for uniqueness of products in the cart
            var duplicates = value
                .GroupBy(cartItem => cartItem.Product)
                .Where(group => group.Count() > 1)
                .ToList();

            if (duplicates.Count > 0)
                throw new ValidationException("Duplicate products in Cart.");

            _cart = value;
            _cart.MakeReadOnly();
        }
    }

    #endregion

    #region ----------< Calculated Properties >----------

    [JsonIgnore] public virtual decimal TotalPrice => _associatedProducts.Sum(item => item.TotalPrice);

    [JsonIgnore] public decimal TotalWeight => _associatedProducts.Sum(item => item.TotalWeight);

    [JsonIgnore] public decimal TotalDimensions => _associatedProducts.Sum(item => item.TotalDimensions);

    #endregion

    #region ----------< Construction >----------

    protected Order(
        int id,
        DateTime creationDate,
        OrderStatus status,
        DeserializableReadOnlyList<ProductEntry> cart)
    {
        Id = id;
        CreationDate = creationDate;
        Status = status;
        Cart = cart;
    }

    [JsonIgnore] private bool IsConstructed { get; set; }

    protected void FinishConstruction()
    {
        if (IsConstructed) return;
        IsConstructed = true;

        // parent-specifics
        CreateProductAssociations();
        Extent.Add(this);

        // child-specifics hook
        OnAfterConstruction();
    }

    protected virtual void OnAfterConstruction()
    {
    }

    #endregion

    #region ----------< Associations >----------

    private readonly HashSet<ProductQuantityInOrder> _associatedProducts = [];

    [JsonIgnore] public HashSet<ProductQuantityInOrder> AssociatedProducts => [.._associatedProducts];

    private void CreateProductAssociations()
    {
        foreach (var cartItem in Cart)
        {
            var association = new ProductQuantityInOrder(cartItem.Product, this, cartItem.Quantity);
            association.Order.AssociateWithProduct(association);
            association.Product.AssociateWithOrder(association);
        }
    }

    public void AssociateWithProduct(ProductQuantityInOrder orderItem)
    {
        orderItem.IsNotNull(nameof(orderItem));
        if (orderItem.Order != this)
            throw new ValidationException($"{nameof(orderItem.Order)} must reference this Order instance.");

        if (_associatedProducts.Add(orderItem))
            orderItem.Product.AssociateWithOrder(orderItem);
    }

    #endregion
}