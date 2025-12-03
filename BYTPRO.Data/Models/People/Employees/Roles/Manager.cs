using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees.Roles;

public class Manager
{
    // ----------< Attributes >----------
    private ManagerialLevel _managerialLevel;


    // ----------< Properties with validation >----------
    [JsonConverter(typeof(StringEnumConverter))]
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