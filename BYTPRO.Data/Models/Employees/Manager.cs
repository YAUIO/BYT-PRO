using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Employees;

public class Manager(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    string pesel,
    decimal salary,
    EmploymentType employmentType,
    ManagerialLevel level,
    IUnitOfWork uow)
    : Employee(id, name, surname, phone, email, password, pesel, salary, employmentType, uow)
{
    public ManagerialLevel ManagerialLevel { get; set; } = level;
}