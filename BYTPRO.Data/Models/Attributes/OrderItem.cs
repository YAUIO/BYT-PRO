namespace BYTPRO.Data.Models.Attributes;

public class OrderItem(Product product, int quantity)
{
    public Product Product { get; set; } = product;
    public int Quantity { get; set; } = quantity;
    
    public decimal OrderPrice { get; set; } = product.Price;

    public decimal TotalPrice => Quantity * OrderPrice;
}