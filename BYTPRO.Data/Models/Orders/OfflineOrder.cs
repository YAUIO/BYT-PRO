using BYTPRO.Data.Models.Branches;

namespace BYTPRO.Data.Models.Orders;

public class OfflineOrder(
    int id,
    DateTime creationDate,
    Store originStore,
    string? phone = null)
    : Order(id, creationDate)
{
    public string? Phone { get; set; } = phone;
    
    public Store OriginStore { get; set; } = originStore;
}