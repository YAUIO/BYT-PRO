using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees.Roles;

public class Manager
{
    #region ----------< Attributes >----------

    private ManagerialLevel _managerialLevel;

    #endregion

    #region ----------< Properties with validation >----------

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

    #endregion

    #region ----------< Constructor >----------

    public Manager(
        ManagerialLevel managerialLevel)
    {
        ManagerialLevel = managerialLevel;
    }

    #endregion
}