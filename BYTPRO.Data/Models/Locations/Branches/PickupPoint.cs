using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class PickupPoint : Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<PickupPoint> Extent => JsonContext.Context.GetTable<PickupPoint>();

    [JsonIgnore] public new static IReadOnlyList<PickupPoint> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private int _parcelStorageSlots;
    private decimal _maxParcelWeight;


    // ----------< Properties with validation >----------
    public int ParcelStorageSlots
    {
        get => _parcelStorageSlots;
        set
        {
            value.IsNonNegative(nameof(ParcelStorageSlots));
            _parcelStorageSlots = value;
        }
    }

    public decimal MaxParcelWeight
    {
        get => _maxParcelWeight;
        set
        {
            value.IsPositive(nameof(MaxParcelWeight));
            _maxParcelWeight = value;
        }
    }


    // ----------< Constructor >----------
    public PickupPoint(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        int parcelStorageSlots,
        decimal maxParcelWeight)
        : base(address, name, openingHours, totalArea)
    {
        ParcelStorageSlots = parcelStorageSlots;
        MaxParcelWeight = maxParcelWeight;

        RegisterBranch();
        Extent.Add(this);
    }

    [JsonConstructor]
    private PickupPoint()
    {
        RegisterBranch();
        Extent.Add(this);
    }


    // ----------< Associations >----------
    public new void Delete()
    {
        base.Delete();
        Extent.Remove(this);
    }
}