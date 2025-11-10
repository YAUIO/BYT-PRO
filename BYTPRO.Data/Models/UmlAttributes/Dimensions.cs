namespace BYTPRO.Data.Models.UmlAttributes;

public record Dimensions(decimal Width, decimal Height, decimal Depth)
{
    public decimal Volume => Width * Height * Depth;
}