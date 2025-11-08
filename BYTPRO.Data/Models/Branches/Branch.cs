using BYTPRO.Data.Models.Attributes;
using BYTPRO.Data.Models.Employees;

namespace BYTPRO.Data.Models.Branches;

public abstract class Branch(
    Address address,
    string name,
    string openingHours,
    decimal totalArea)
{
    public Address Address { get; set; } = address;
    public string Name { get; set; } = name;
    public string OpeningHours { get; set; } = openingHours;
    public decimal TotalArea { get; set; } = totalArea;

    public List<Employee> Employees { get; set; } = new List<Employee>();
    
    public List<BranchStock> Stock { get; set; } = new List<BranchStock>();
    
}