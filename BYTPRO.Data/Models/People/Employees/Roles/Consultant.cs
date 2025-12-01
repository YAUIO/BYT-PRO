using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People.Employees.Roles;

public class Consultant
{
    // ----------< Attributes >----------
    private string _specialization;
    private readonly DeserializableReadOnlyList<string> _languages;


    // ----------< Properties with validation >----------
    public string Specialization
    {
        get => _specialization;
        set
        {
            value.IsNotNullOrEmpty(nameof(Specialization));
            _specialization = value;
        }
    }

    public DeserializableReadOnlyList<string> Languages
    {
        get => _languages;
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(Languages));
            _languages = value;
            _languages.MakeReadOnly();
        }
    }


    // ----------< Constructor >----------
    public Consultant(
        string specialization,
        DeserializableReadOnlyList<string> languages
    )
    {
        Specialization = specialization;
        Languages = languages;
    }
}