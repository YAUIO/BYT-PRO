using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.People.Employees;

public abstract class Employee : Person
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static readonly List<Employee> Extent = [];

    [JsonIgnore] public new static IReadOnlyList<Employee> All => Extent.ToList().AsReadOnly();

    protected void RegisterEmployee() => Extent.Add(this);


    // ----------< Attributes >----------
    private readonly string _pesel;
    private decimal _salary;
    private EmploymentType _employmentType;


    // ----------< Properties with validation >----------
    public string Pesel
    {
        get => _pesel;
        init
        {
            value.IsPesel();
            _pesel = value;
        }
    }

    public decimal Salary
    {
        get => _salary;
        set
        {
            value.IsPositive(nameof(Salary));
            _salary = value;
        }
    }

    public EmploymentType EmploymentType
    {
        get => _employmentType;
        set
        {
            value.IsDefined(nameof(EmploymentType));
            _employmentType = value;
        }
    }


    // ----------< Constructor >----------
    protected Employee(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType)
        : base(id, name, surname, phone, email, password)
    {
        Pesel = pesel;
        Salary = salary;
        EmploymentType = employmentType;
    }
}