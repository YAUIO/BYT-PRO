using Newtonsoft.Json;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.People;

public abstract class Person
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static readonly List<Person> Extent = [];

    [JsonIgnore] public static IReadOnlyList<Person> All => Extent.ToList().AsReadOnly();

    protected void DeletePerson() => Extent.Remove(this);

    #endregion

    #region ----------< Attributes >----------

    private readonly int _id;
    private readonly string _name;
    private readonly string _surname;
    private string _phone;
    private string _email;
    private string _password;

    #endregion

    #region ----------< Properties with validation >----------

    public int Id
    {
        get => _id;
        init
        {
            value.IsNonNegative(nameof(Id));
            if (Extent.Any(p => p.Id == value))
                throw new ValidationException($"Person with Id {value} already exists.");
            _id = value;
        }
    }

    public string Name
    {
        get => _name;
        init
        {
            value.IsNotNullOrEmpty(nameof(Name));
            value.IsBelowMaxLength(50);
            _name = value;
        }
    }

    public string Surname
    {
        get => _surname;
        init
        {
            value.IsNotNullOrEmpty(nameof(Surname));
            value.IsBelowMaxLength(50);
            _surname = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            value.IsPhoneNumber();
            _phone = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            value.IsEmail();
            if (Extent.Any(p => p.Email.Equals(value)))
                throw new ValidationException($"Email '{value}' already exists.");
            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            value.IsNotNullOrEmpty(nameof(Password));
            value.IsBelowMaxLength(100);
            _password = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    protected Person(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Phone = phone;
        Email = email;
        Password = password;
    }

    [JsonIgnore] private bool IsConstructed { get; set; }

    protected void FinishConstruction()
    {
        if (IsConstructed) return;
        IsConstructed = true;

        // parent-specifics
        Extent.Add(this);

        // child-specifics hook
        OnAfterConstruction();
    }

    protected virtual void OnAfterConstruction()
    {
    }

    #endregion
}