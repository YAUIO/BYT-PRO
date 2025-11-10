namespace BYTPRO.Data.Models.UmlAttributes;

public record OrderItem(Product Product, int Quantity)
{
    public decimal OrderPrice => Product.Price;

    public decimal TotalPrice => Quantity * OrderPrice;
}