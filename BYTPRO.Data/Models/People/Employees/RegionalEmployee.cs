using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People.Employees;

public class RegionalEmployee : Employee
{
    // ----------< Class Extent >----------
    [JsonIgnore]
    private static JsonEntitySet<RegionalEmployee> Extent => JsonContext.Context.GetTable<RegionalEmployee>();

    [JsonIgnore] public new static IReadOnlyList<RegionalEmployee> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly string _badgeNumber;
    private SupervisionScope _supervisionScope;


    // ----------< Properties with validation >----------
    public string BadgeNumber
    {
        get => _badgeNumber;
        init
        {
            value.IsNotNullOrEmpty(nameof(BadgeNumber));
            _badgeNumber = value;
        }
    }

    public SupervisionScope SupervisionScope
    {
        get => _supervisionScope;
        set
        {
            value.IsDefined(nameof(SupervisionScope));
            _supervisionScope = value;
        }
    }


    // ----------< Constructor >----------
    public RegionalEmployee(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType,
        string badgeNumber,
        SupervisionScope supervisionScope
    ) : base(id, name, surname, phone, email, password, pesel, salary, employmentType)
    {
        BadgeNumber = badgeNumber;
        SupervisionScope = supervisionScope;

        // IMPORTANT NOTE:
        // We defer registration to super class (adding to super Class Extent)
        // until all properties are validated and set for child class.
        RegisterPerson();
        RegisterEmployee();
        Extent.Add(this);
    }
}