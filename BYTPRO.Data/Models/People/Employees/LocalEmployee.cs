using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People.Employees;

public class LocalEmployee : Employee
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<LocalEmployee> Extent => JsonContext.Context.GetTable<LocalEmployee>();

    [JsonIgnore] public new static IReadOnlyList<LocalEmployee> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private readonly List<string> _trainingsCompleted = [];
    private string _breakSchedule;


    // ----------< Properties with validation >----------
    public ReadOnlyCollection<string> TrainingsCompleted
    {
        get => _trainingsCompleted.AsReadOnly();
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(TrainingsCompleted));
            _trainingsCompleted.AddRange(value);
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


    // ----------< Constructor >----------
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
        List<string> trainingsCompleted,
        string breakSchedule
    ) : base(id, name, surname, phone, email, password, pesel, salary, employmentType)
    {
        TrainingsCompleted = new ReadOnlyCollection<string>(trainingsCompleted);
        BreakSchedule = breakSchedule;

        // IMPORTANT NOTE:
        // We defer registration to super class (adding to super Class Extent)
        // until all properties are validated and set for child class.
        RegisterPerson();
        RegisterEmployee();
        Extent.Add(this);
    }
}