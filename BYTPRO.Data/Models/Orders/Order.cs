using BYTPRO.Data.Models.Attributes;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Orders;

public abstract class Order(int id, DateTime creationDate)
{
    public int Id { get; set; } = id;
    public DateTime CreationDate { get; set; } = creationDate;
    public OrderStatus Status { get; set; } = OrderStatus.InProgress;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    public virtual decimal TotalPrice => OrderItems.Sum(item => item.TotalPrice);

    public decimal TotalWeight => OrderItems.Sum(item => item.Product.Weight * item.Quantity);
    
    public decimal TotalDimensions => OrderItems.Sum(item => item.Product.Dimensions.Volume * item.Quantity);


    public void Cancel()
    {
        this.Status = OrderStatus.Cancelled;
        Console.WriteLine($"Order {Id} has been cancelled.");
    }

    public void Issue()
    {
        Console.WriteLine($"Issuing order {Id}.");
    }
    
    public void Return()
    {
        this.Status = OrderStatus.Returned;
        Console.WriteLine($"Order {Id} is being returned.");
    }

    public virtual void ViewDetails()
    {
        Console.WriteLine($"Order ID: {Id}, Status: {Status}, Total Price: {TotalPrice}");
        foreach (var item in OrderItems)
        {
            Console.WriteLine($"  - {item.Product.Name} (x{item.Quantity}) @ {item.OrderPrice:C}");
        }
    }
    
    public static List<Order> GetHistoryByCustomer(Customer customer)
    {
        Console.WriteLine($"Getting order history for {customer.Name}");
        return new List<Order>(); 
    }

    public static Order? FindById(int id)
    {
        Console.WriteLine($"Finding order by ID: {id}");
        return null;
    }
}