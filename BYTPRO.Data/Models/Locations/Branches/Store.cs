using System.Text.Json.Serialization;
using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.Locations.Branches;

public class Store : Branch
{
    // ----------< Class Extent >----------
    [JsonIgnore] private static JsonEntitySet<Store> Extent => JsonContext.Context.GetTable<Store>();

    [JsonIgnore] public new static IReadOnlyList<Store> All => Extent.ToList().AsReadOnly();


    // ----------< Attributes >----------
    private int _posCount;
    private decimal _salesArea;
    private int _selfCheckouts;


    // ----------< Properties with validation >----------
    public int PosCount
    {
        get => _posCount;
        set
        {
            value.IsNonNegative(nameof(PosCount));
            _posCount = value;
        }
    }

    public decimal SalesArea
    {
        get => _salesArea;
        set
        {
            value.IsPositive(nameof(SalesArea));
            if (value > TotalArea)
                throw new ValidationException($"{nameof(SalesArea)} cannot exceed {nameof(TotalArea)}");
            _salesArea = value;
        }
    }

    public int SelfCheckouts
    {
        get => _selfCheckouts;
        set
        {
            value.IsNonNegative(nameof(SelfCheckouts));
            _selfCheckouts = value;
        }
    }


    // ----------< Constructor >----------
    public Store(
        Address address,
        string name,
        string openingHours,
        decimal totalArea,
        int posCount,
        decimal salesArea,
        int selfCheckouts)
        : base(address, name, openingHours, totalArea)
    {
        PosCount = posCount;
        SalesArea = salesArea;
        SelfCheckouts = selfCheckouts;

        RegisterBranch();
        Extent.Add(this);
    }
}