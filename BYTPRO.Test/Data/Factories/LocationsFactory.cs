using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;

namespace BYTPRO.Test.Data.Factories;

internal static class LocationsFactory
{
    #region ----------< Default values >----------

    private static Address DefaultAddress() =>
        new(
            "Test Street",
            "14/c",
            null,
            "00000",
            "Test City"
        );

    private static string DefaultHours() => "08:00–22:00";

    #endregion

    #region ----------< Branch Factory methods >----------

    public static PickupPoint CreatePickupPoint(
        // ----------< Branch >----------
        Address? address = null,
        string name = "Test PickupPoint",
        string? openingHours = null,
        decimal totalArea = 100,
        // ----------< PickupPoint >----------
        int parcelStorageSlots = 50,
        decimal maxParcelWeight = 20)
    {
        return new PickupPoint(
            address ?? DefaultAddress(),
            name,
            openingHours ?? DefaultHours(),
            totalArea,
            parcelStorageSlots,
            maxParcelWeight);
    }

    public static Store CreateStore(
        // ----------< Branch >----------
        Address? address = null,
        string name = "Test Store",
        string? openingHours = null,
        decimal totalArea = 500,
        // ----------< Store >----------
        int posCount = 4,
        decimal salesArea = 300,
        int selfCheckouts = 2)
    {
        return new Store(
            address ?? DefaultAddress(),
            name,
            openingHours ?? DefaultHours(),
            totalArea,
            posCount,
            salesArea,
            selfCheckouts
        );
    }

    public static Warehouse CreateWarehouse(
        // ----------< Branch >----------
        Address? address = null,
        string name = "Test Warehouse",
        string? openingHours = null,
        decimal totalArea = 2000,
        // ----------< Warehouse >----------
        decimal maxStorageCapacity = 5000,
        int dockCount = 3)
    {
        return new Warehouse(
            address ?? DefaultAddress(),
            name,
            openingHours ?? DefaultHours(),
            totalArea,
            maxStorageCapacity,
            dockCount
        );
    }

    #endregion

    #region ----------< Pure Branch Factory methods >----------

    public sealed class PureBranch : Branch
    {
        public PureBranch(
            Address address,
            string name,
            string openingHours,
            decimal totalArea)
            : base(address, name, openingHours, totalArea)
        {
            FinishConstruction();
        }
    }

    public static PureBranch CreatePureBranch(
        // ----------< Branch >----------
        Address? address = null,
        string name = "Pure Branch",
        string? openingHours = null,
        decimal totalArea = 200M)
    {
        return new PureBranch(
            address ?? DefaultAddress(),
            name,
            openingHours ?? DefaultHours(),
            totalArea
        );
    }

    #endregion
}