using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Orders;

public class BranchOrder(
    int id,
    DateTime creationDate,
    DateTime expectedDeliveryDate)
    : Order(id, creationDate)
{
    public DateTime ExpectedDeliveryDate { get; set; } = expectedDeliveryDate;

    public void MarkAsDelivered() // TODO move data storing to persistence, out of models, and business logic to services
    {
        Status = OrderStatus.Completed;
        Console.WriteLine($"Branch order {Id} marked as delivered.");
    }
    
    public override decimal TotalPrice
    {
        get
        {
            return 0m;
        }
    }
}