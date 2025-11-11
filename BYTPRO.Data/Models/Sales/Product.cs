namespace BYTPRO.Data.Models.Sales;

public class Product(
    string name,
    string description,
    decimal price,
    List<string> images,
    decimal weight,
    Dimensions dimensions)
{
    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public List<string> Images { get; set; } = images;

    public decimal Weight { get; set; } = weight;

    public Dimensions Dimensions { get; init; } = dimensions;
}