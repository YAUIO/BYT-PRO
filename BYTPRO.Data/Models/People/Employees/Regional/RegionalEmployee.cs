using System.Runtime.Serialization;
using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees.Regional;

public class RegionalEmployee : Employee
{
    // ----------< Class Extent >----------
    [JsonIgnore]
    private static HashSet<RegionalEmployee> Extent => JsonContext.Context.GetTable<RegionalEmployee>();

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

    [JsonConverter(typeof(StringEnumConverter))]
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
    
    [JsonConstructor]
    private RegionalEmployee(
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType,
        string badgeNumber,
        SupervisionScope supervisionScope,
        int id
    ) : base(id, name, surname, phone, email, password, pesel, salary, employmentType)
    {
        BadgeNumber = badgeNumber;
        SupervisionScope = supervisionScope;

        
    }
    
    [OnDeserialized]
    internal void Register(StreamingContext context)
    {
        if (Extent.Any(c => c.Id == Id))
            return;
        
        RegisterPerson();
        RegisterEmployee();
        Extent.Add(this);
    }
}