using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models.Branches;
using BYTPRO.Data.Models.Employees;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models;

public class LocalEmployee(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    string pesel,
    decimal salary,
    EmploymentType employmentType,
    string breakSchedule,
    IUnitOfWork uow)
    : Employee(id, name, surname, phone, email, password, pesel, salary, employmentType)
{
    public List<string> TrainingsCompleted { get; set; } = [];
    
    public string BreakSchedule { get; set; } = breakSchedule;

    public Branch WorksAt { get; set; }
}