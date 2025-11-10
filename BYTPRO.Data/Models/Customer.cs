using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models;

public class Customer : Person
{
    // ----------< Class Extent >----------
    private readonly JsonEntitySet<Customer> _extent;
    public new IReadOnlyList<Customer> All => _extent.ToList().AsReadOnly();


    // ----------< Constants / Business Rules >----------
    public static readonly decimal LoyaltyDiscountPercentage = 0.03m;

    // ----------< Attributes >----------
    private readonly DateTime _registrationDate;

    // ----------< Properties with validation >----------
    public DateTime RegistrationDate
    {
        get => _registrationDate;
        init
        {
            value.IsNotDefault(nameof(RegistrationDate));
            _registrationDate = value;
        }
    }

    public bool IsLoyal => RegistrationDate.AddYears(2) <= DateTime.Today /*&& SuccessfulOrders() > 12*/;

    // ----------< Constructor >----------
    public Customer(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        DateTime registrationDate,
        JsonContext context)
        : base(id, name, surname, phone, email, password)
    {
        RegistrationDate = registrationDate;
        _extent = context.GetTable<Customer>();

        RegisterPerson();
        _extent.Add(this);
    }

    // ----------< Methods >----------
    public override string ToString() => $"{base.ToString()}, {RegistrationDate}, Loyal: {IsLoyal}";
}