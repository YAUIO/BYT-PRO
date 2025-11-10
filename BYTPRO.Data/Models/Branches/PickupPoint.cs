using BYTPRO.Data.Models.Orders;
using BYTPRO.Data.Models.UmlAttributes;

namespace BYTPRO.Data.Models.Branches;

public class PickupPoint(
    Address address,
    string name,
    string openingHours,
    decimal totalArea,
    int parcelStorageSlots,
    decimal maxParcelWeight)
    : Branch(address, name, openingHours, totalArea)
{
    public int ParcelStorageSlots { get; set; } = parcelStorageSlots;
    public decimal MaxParcelWeight { get; set; } = maxParcelWeight;

    public List<OnlineOrder> OnlineOrders { get; set; } = new List<OnlineOrder>();
    
}