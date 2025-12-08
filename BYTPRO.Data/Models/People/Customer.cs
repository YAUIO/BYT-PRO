using Newtonsoft.Json;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People;

public class Customer : Person
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<Customer> Extent => JsonContext.Context.GetTable<Customer>();

    [JsonIgnore] public new static IReadOnlyList<Customer> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Constants / Business Rules >----------

    public static readonly decimal LoyaltyDiscountPercentage = 0.03m;

    #endregion

    #region ----------< Attributes >----------

    private readonly DateTime _registrationDate;

    #endregion

    #region ----------< Properties with validation >----------

    public DateTime RegistrationDate
    {
        get => _registrationDate;
        init
        {
            value.IsNotDefault(nameof(RegistrationDate));
            value.IsBefore(DateTime.Now, nameof(RegistrationDate), "Now");
            _registrationDate = value;
        }
    }

    #endregion

    #region ----------< Calculated Properties >----------

    [JsonIgnore] public bool IsLoyal => RegistrationDate.AddYears(2) <= DateTime.Today /*&& SuccessfulOrders() > 12*/;

    #endregion

    #region ----------< Construction >----------

    public Customer(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        DateTime registrationDate)
        : base(id, name, surname, phone, email, password)
    {
        RegistrationDate = registrationDate;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    #region -----< Qualified >-----

    private readonly Dictionary<string, OnlineOrder> _ordersByTrackingNumber = [];

    [JsonIgnore] public Dictionary<string, OnlineOrder> OnlineOrders => new(_ordersByTrackingNumber);

    internal void AddOrder(OnlineOrder order)
    {
        order.IsNotNull(nameof(order));
        if (order.Customer != this)
            throw new ValidationException($"{nameof(order.Customer)} must reference this Customer instance.");

        _ordersByTrackingNumber.Add(order.TrackingNumber, order);
    }

    #endregion

    protected override void OnDelete()
    {
        Extent.Remove(this);
    }

    #endregion
}