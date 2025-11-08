using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Employees;

public abstract class Cashier(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    string pesel,
    decimal salary,
    EmploymentType employmentType,
    int registerCode,
    int pinCode,
    bool canMakeReturn)
    : Employee(id, name, surname, phone, email, password, pesel, salary, employmentType)
{
    public int RegisterCode { get; set; } = registerCode;
    public int PinCode { get; set; } = pinCode;
    public bool CanMakeReturn { get; } = canMakeReturn;
}