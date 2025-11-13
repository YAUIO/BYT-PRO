using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.People.Employees.Roles;

public class Manager
{
    // ----------< Attributes >----------
    private ManagerialLevel _managerialLevel;


    // ----------< Properties with validation >----------
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ManagerialLevel ManagerialLevel
    {
        get => _managerialLevel;
        set
        {
            value.IsDefined(nameof(ManagerialLevel));
            _managerialLevel = value;
        }
    }


    // ----------< Constructor >----------
    public Manager(ManagerialLevel managerialLevel)
    {
        ManagerialLevel = managerialLevel;
    }
}