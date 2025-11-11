namespace BYTPRO.Data.Models.Locations.Branches;

public class Store(
    Address address,
    string name,
    string openingHours,
    decimal totalArea,
    int posCount,
    decimal salesArea,
    int selfCheckouts)
    : Branch(address, name, openingHours, totalArea)
{
    public int PosCount { get; set; } = posCount;

    public decimal SalesArea { get; set; } = salesArea;

    public int SelfCheckouts { get; set; } = selfCheckouts;
}