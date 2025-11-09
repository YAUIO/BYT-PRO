using BYTPRO.Data.Models.Branches;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Orders;

public class OnlineOrder(
    int id,
    DateTime creationDate,
    Customer customer,
    string trackingNumber,
    PickupPoint destinationPickupPoint)
    : Order(id, creationDate)
{
    public bool IsPaid { get; set; }
    public DateTime? CancellationDate { get; private set; }
    
    public string TrackingNumber { get; set; } = trackingNumber;
    
    public PickupPoint DestinationPickupPoint { get; set; } = destinationPickupPoint;
    
    public void PayExternally() // TODO move data storing to persistence, out of models, and business logic to services
    {
        IsPaid = true;
        Console.WriteLine($"Order {Id} marked as paid externally.");
    }

    public void MarkAsDelivered() // TODO move data storing to persistence, out of models, and business logic to services
    {
        Status = OrderStatus.Completed;
        Console.WriteLine($"Order {Id} marked as delivered.");
    }

    public void AutoCancelAwaiting() // TODO move data storing to persistence, out of models, and business logic to services
    {
        if (Status != OrderStatus.AwaitingCollection) return;

        Status = OrderStatus.Cancelled;
        CancellationDate = DateTime.Now;
        Console.WriteLine($"Order {Id} auto-cancelled.");
    }
}