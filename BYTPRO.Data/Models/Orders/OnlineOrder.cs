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
    
    public void PayExternally()
    {
        IsPaid = true;
        Console.WriteLine($"Order {Id} marked as paid externally.");
    }

    public void MarkAsDelivered()
    {
        Status = OrderStatus.Completed;
        Console.WriteLine($"Order {Id} marked as delivered.");
    }

    public void AutoCancelAwaiting()
    {
        if (Status == OrderStatus.AwaitingCollection)
        {
            Status = OrderStatus.Cancelled;
            CancellationDate = DateTime.Now;
            Console.WriteLine($"Order {Id} auto-cancelled.");
        }
    }
}