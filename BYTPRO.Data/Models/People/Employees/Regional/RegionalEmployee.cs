using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees.Regional;

public class RegionalEmployee : Employee
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<RegionalEmployee> Extent => JsonContext.Context.GetTable<RegionalEmployee>();

    [JsonIgnore] public new static IReadOnlyList<RegionalEmployee> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly string _badgeNumber;
    private SupervisionScope _supervisionScope;

    #endregion

    #region ----------< Properties with validation >----------

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

    #endregion

    #region ----------< Construction >----------

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
        CashierParams? cashier,
        ConsultantParams? consultant,
        ManagerParams? manager,
        string badgeNumber,
        SupervisionScope supervisionScope
    ) : base(id, name, surname, phone, email, password, pesel, salary, employmentType, cashier, consultant, manager)
    {
        BadgeNumber = badgeNumber;
        SupervisionScope = supervisionScope;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        // since this is the third level class in an inheritance tree,
        // we need to call for the second one (Employee class)
        base.OnAfterConstruction();

        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    protected override void OnDelete()
    {
        // since this is the third level class in an inheritance tree,
        // we need to call for the second one (Employee class)
        base.OnDelete();

        Extent.Remove(this);
    }

    #endregion
}