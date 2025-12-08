using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees;

public abstract class Employee : Person
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static readonly List<Employee> Extent = [];

    [JsonIgnore] public new static IReadOnlyList<Employee> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly string _pesel;
    private decimal _salary;
    private EmploymentType _employmentType;

    #endregion

    #region ----------< Properties with validation >----------

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

    [JsonConverter(typeof(StringEnumConverter))]
    public EmploymentType EmploymentType
    {
        get => _employmentType;
        set
        {
            value.IsDefined(nameof(EmploymentType));
            _employmentType = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

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

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    protected override void OnDelete()
    {
        Extent.Remove(this);
    }

    #endregion
}