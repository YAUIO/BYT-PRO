using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Factories;

internal static class SalesFactory
{
    #region ----------< Default values >----------

    // Product
    private static string DefaultProductName() => "Test Product";
    private static string DefaultProductDescription() => "Test Product Description.";
    private static decimal DefaultProductPrice() => 100m;
    private static DeserializableReadOnlyList<string> DefaultImages() => ["image1.png", "image2.png"];
    private static decimal DefaultWeight() => 2m;
    private static Dimensions DefaultDimensions() => new(10, 5, 2);

    // Order
    private static int _orderIdCounter = 1;
    private static int DefaultOrderId() => Interlocked.Increment(ref _orderIdCounter);
    private static OrderStatus DefaultOrderStatus() => OrderStatus.InProgress;
    private static DateTime DefaultOrderDate() => DateTime.UtcNow;

    // BranchOrder
    private static DateTime DefaultExpectedDeliveryDate() => DefaultOrderDate().AddDays(2);

    #endregion

    #region ----------< Product Factory methods >----------

    public static Product CreateProduct(
        string? name = null,
        string? description = null,
        decimal? price = null,
        DeserializableReadOnlyList<string>? images = null,
        decimal? weight = null,
        Dimensions? dimensions = null,
        HashSet<Product>? consistsOf = null)
    {
        return new Product(
            name ?? DefaultProductName(),
            description ?? DefaultProductDescription(),
            price ?? DefaultProductPrice(),
            images ?? DefaultImages(),
            weight ?? DefaultWeight(),
            dimensions ?? DefaultDimensions(),
            consistsOf
        );
    }

    #endregion

    #region ----------< Order Factory methods >----------

    public static OnlineOrder CreateOnlineOrder(
        // ----------< Order (required) >----------
        DeserializableReadOnlyList<ProductEntry> cart,
        // ----------< OnlineOrder (required) >----------
        Customer customer,
        PickupPoint pickupPoint,
        // ----------< Order >----------
        int? id = null,
        DateTime? creationDate = null,
        OrderStatus? status = null,
        // ----------< OnlineOrder >----------
        bool isPaid = false,
        DateTime? cancellationDate = null,
        string? trackingNumber = null)
    {
        var realId = id ?? DefaultOrderId();
        return new OnlineOrder(
            realId,
            creationDate ?? DefaultOrderDate(),
            status ?? DefaultOrderStatus(),
            cart,
            isPaid,
            cancellationDate,
            trackingNumber ?? $"TRACK{realId:D10}",
            customer,
            pickupPoint
        );
    }

    public static OfflineOrder CreateOfflineOrder(
        // ----------< Order (required) >----------
        DeserializableReadOnlyList<ProductEntry> cart,
        // ----------< OfflineOrder (required) >----------
        Store store,
        // ----------< Order >----------
        int? id = null,
        DateTime? creationDate = null,
        OrderStatus? status = null,
        // ----------< OfflineOrder >----------
        string? phone = null)
    {
        var realId = id ?? DefaultOrderId();
        return new OfflineOrder(
            realId,
            creationDate ?? DefaultOrderDate(),
            status ?? DefaultOrderStatus(),
            cart,
            phone,
            store
        );
    }

    public static BranchOrder CreateBranchOrder(
        // ----------< Order (required) >----------
        DeserializableReadOnlyList<ProductEntry> cart,
        // ----------< BranchOrder (required) >----------
        Warehouse from,
        Branch to,
        // ----------< Order >----------
        int? id = null,
        DateTime? creationDate = null,
        OrderStatus? status = null,
        // ----------< BranchOrder >----------
        DateTime? expectedDeliveryDate = null)
    {
        var realId = id ?? DefaultOrderId();
        return new BranchOrder(
            realId,
            creationDate ?? DefaultOrderDate(),
            status ?? DefaultOrderStatus(),
            cart,
            expectedDeliveryDate ?? DefaultExpectedDeliveryDate(),
            from,
            to
        );
    }

    #endregion
}