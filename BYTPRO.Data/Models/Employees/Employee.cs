using System.Text.Json.Serialization;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Employees;

public abstract class Employee : Person
{
    // ----------< Class extent >----------
    [JsonIgnore]
    private static readonly List<Person> Extent = [];
    
    [JsonIgnore]
    public new static IReadOnlyList<Person> All => Extent.ToList().AsReadOnly();

    private string _pesel;
    
    public string Pesel
    {
        get => _pesel;
        set
        {
            value.IsPesel();
            _pesel = value;
        }
    }
    
    public decimal Salary { get; set; }
    
    public EmploymentType EmploymentType { get; set; }

    protected Employee(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType)
        : base(id, name, surname, phone, email, password)
    {
        Pesel = pesel;
        Salary = salary;
        EmploymentType = employmentType;
        
        Extent.Add(this);
    }
    
    public void ChangeEmploymentType(EmploymentType newType) // TODO move data storing to persistence, out of models
    {
        this.EmploymentType = newType;
        Console.WriteLine($"Employee {Name} {Surname} new employment type: {newType}");
    }
}