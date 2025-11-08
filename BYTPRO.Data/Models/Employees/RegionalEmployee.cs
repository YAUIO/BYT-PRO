using BYTPRO.Data.Models.Employees;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models;

public class RegionalEmployee(
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
    SupervisionScope scope)
    : Employee(id, name, surname, phone, email, password, pesel, salary, employmentType)
{
    public string BadgeNumber { get; set; } = badgeNumber;
    public SupervisionScope SupervisionScope { get; set; } = scope;

    
}