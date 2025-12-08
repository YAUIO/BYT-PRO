using Newtonsoft.Json;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People.Employees.Local;

public class LocalEmployee : Employee
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static HashSet<LocalEmployee> Extent => JsonContext.Context.GetTable<LocalEmployee>();

    [JsonIgnore] public new static IReadOnlyList<LocalEmployee> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly DeserializableReadOnlyList<string> _trainingsCompleted;
    private string _breakSchedule;

    #endregion

    #region ----------< Properties with validation >----------

    public DeserializableReadOnlyList<string> TrainingsCompleted
    {
        get => _trainingsCompleted;
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(TrainingsCompleted));
            _trainingsCompleted = value;
            _trainingsCompleted.MakeReadOnly();
        }
    }

    public string BreakSchedule
    {
        get => _breakSchedule;
        set
        {
            value.IsNotNullOrEmpty(nameof(BreakSchedule));
            _breakSchedule = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    public LocalEmployee(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType,
        DeserializableReadOnlyList<string> trainingsCompleted,
        string breakSchedule,
        Branch branch
    ) : base(id, name, surname, phone, email, password, pesel, salary, employmentType)
    {
        TrainingsCompleted = trainingsCompleted;
        BreakSchedule = breakSchedule;
        Branch = branch;

        FinishConstruction();
    }

    protected override void OnAfterConstruction()
    {
        // since this is the third level class in an inheritance tree,
        // we need to call for the second one (Employee class)
        base.OnAfterConstruction();

        Branch.AddEmployee(this);

        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    private readonly Branch _branch;

    public Branch Branch
    {
        get => _branch;
        init
        {
            value.IsNotNull(nameof(Branch));
            _branch = value;
        }
    }

    public void Delete()
    {
        Branch.RemoveEmployee(this);

        Extent.Remove(this);

        DeleteEmployee();
        DeletePerson();
    }

    #endregion
}