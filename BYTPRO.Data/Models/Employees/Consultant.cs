using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Employees;

public class Consultant(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    string pesel,
    decimal salary,
    EmploymentType employmentType,
    string specialization)
    : Employee(id, name, surname, phone, email, password, pesel, salary, employmentType)
{
    public string Specialization { get; set; } = specialization;
    public List<string> Languages { get; set; } = new List<string>();
}