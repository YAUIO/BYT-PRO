namespace BYTPRO.Data.Models.Locations.Branches;

public abstract class Branch(
    Address address,
    string name,
    string openingHours,
    decimal totalArea)
{
    public Address Address { get; init; } = address;

    public string Name { get; set; } = name;

    public string OpeningHours { get; set; } = openingHours;

    public decimal TotalArea { get; init; } = totalArea;
}