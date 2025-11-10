using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Employees.Roles;

public class Manager
{
    // ----------< Attributes >----------
    private ManagerialLevel _managerialLevel;


    // ----------< Properties with validation >----------
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