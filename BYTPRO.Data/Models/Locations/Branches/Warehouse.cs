namespace BYTPRO.Data.Models.Locations.Branches;

public class Warehouse(
    Address address,
    string name,
    string openingHours,
    decimal totalArea,
    decimal maxStorageCapacity,
    int dockCount)
    : Branch(address, name, openingHours, totalArea)
{
    public decimal MaxStorageCapacity { get; set; } = maxStorageCapacity;
    public int DockCount { get; set; } = dockCount;

    public decimal CurrentStorageCapacity { get; private set; } = 0;
}