using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Employees;

public abstract class Employee(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    string pesel,
    decimal salary,
    EmploymentType employmentType)
    : Person(id, name, surname, phone, email, password)
{
    public string Pesel { get; set; } = pesel;
    public decimal Salary { get; set; } = salary;
    public EmploymentType EmploymentType { get; set; } = employmentType;
    
    public void ChangeEmploymentType(EmploymentType newType)
    {
        this.EmploymentType = newType;
        Console.WriteLine($"Employee {Name} {Surname} new employment type: {newType}");
    }
}