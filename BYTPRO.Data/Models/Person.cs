using System.Text.Json.Serialization;
using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models;

public abstract class Person
{
    // ----------< Class extent >----------
    [JsonIgnore]
    private static readonly List<Person> Extent = [];
    
    [JsonIgnore]
    public static IReadOnlyList<Person> All => Extent.ToList().AsReadOnly();

    protected void RegisterPerson() => Extent.Add(this);
    
    // ----------< Attributes >----------
    private readonly int _id;
    private readonly string _name;
    private readonly string _surname;
    private string _phone;
    private string _email;
    private string _password;

    // ----------< Properties with validation >----------
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

    // ----------< Constructor >----------
    public Person(
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
        
        Extent.Add(this);
    }

    public override bool Equals(object? obj)
    {
        if (obj.GetType() != GetType()) return false;
        var person = (Person) obj;
        return Equals(person);
    }

    private bool Equals(Person other)
    {
        return _id == other._id && _name == other._name && _surname == other._surname && _phone == other._phone && _email == other._email && _password == other._password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _name, _surname);
    }
}