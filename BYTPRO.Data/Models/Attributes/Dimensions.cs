namespace BYTPRO.Data.Models.Attributes;

public class Dimensions(decimal width, decimal height, decimal depth)
{
    public decimal Width { get; set; } = width;
    public decimal Height { get; set; } = height;
    public decimal Depth { get; set; } = depth;

    public decimal Volume => Width * Height * Depth;
}