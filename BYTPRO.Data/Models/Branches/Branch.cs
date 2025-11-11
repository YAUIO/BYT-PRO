using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.UmlAttributes;

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

    public List<Employee> Employees { get; set; } = [];

    public List<BranchStock> Stock { get; set; } = [];
}