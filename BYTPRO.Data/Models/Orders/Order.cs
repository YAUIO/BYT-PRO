using BYTPRO.Data.Models.Attributes;
using BYTPRO.Data.Models.Enums;

namespace BYTPRO.Data.Models.Orders;

public abstract class Order(int id, DateTime creationDate)
{
    
    public int Id { get; set; } = id;
    
    public DateTime CreationDate { get; set; } = creationDate;
    
    public OrderStatus Status { get; set; } = OrderStatus.InProgress;

    public List<OrderItem> OrderItems { get; set; } = [];
    
    public virtual decimal TotalPrice => OrderItems.Sum(item => item.TotalPrice);

    public decimal TotalWeight => OrderItems.Sum(item => item.Product.Weight * item.Quantity);
    
    public decimal TotalDimensions => OrderItems.Sum(item => item.Product.Dimensions.Volume * item.Quantity);


    public void Cancel() // TODO move data storing to persistence, out of models, and business logic to services
    {
        this.Status = OrderStatus.Cancelled;
        Console.WriteLine($"Order {Id} has been cancelled.");
    }

    public void Issue() // TODO move data storing to persistence, out of models, and business logic to services
    {
        Console.WriteLine($"Issuing order {Id}.");
    }
    
    public void Return() // TODO move data storing to persistence, out of models, and business logic to services
    {
        this.Status = OrderStatus.Returned;
        Console.WriteLine($"Order {Id} is being returned.");
    }

    public virtual void ViewDetails() // TODO move data storing to persistence, out of models, and business logic to services
    {
        Console.WriteLine($"Order ID: {Id}, Status: {Status}, Total Price: {TotalPrice}");
        foreach (var item in OrderItems)
        {
            Console.WriteLine($"  - {item.Product.Name} (x{item.Quantity}) @ {item.OrderPrice:C}");
        }
    }
    
    public static List<Order> GetHistoryByCustomer(Customer customer) // TODO move data storing to persistence, out of models, and business logic to services
    {
        Console.WriteLine($"Getting order history for {customer.Name}");
        return new List<Order>(); 
    }

    public static Order? FindById(int id) // TODO move data storing to persistence, out of models, and business logic to services
    {
        Console.WriteLine($"Finding order by ID: {id}");
        return null;
    }
}