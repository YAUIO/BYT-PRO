using System.Collections.ObjectModel;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.People.Employees.Roles;

public class Consultant
{
    // ----------< Attributes >----------
    private string _specialization;
    private readonly List<string> _languages = [];


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

    public ReadOnlyCollection<string> Languages
    {
        get => _languages.AsReadOnly();
        init
        {
            value.AreAllStringsNotNullOrEmpty(nameof(Languages));
            _languages.AddRange(value);
        }
    }


    // ----------< Constructor >----------
    public Consultant(
        string specialization,
        List<string> languages
    )
    {
        Specialization = specialization;
        Languages = new ReadOnlyCollection<string>(languages);
    }
}