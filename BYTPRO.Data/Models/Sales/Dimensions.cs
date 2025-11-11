using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record Dimensions
{
    // ----------< Properties >----------
    public decimal Width { get; }
    public decimal Height { get; }
    public decimal Depth { get; }


    // ----------< Calculated Properties >----------
    [JsonIgnore] public decimal Volume => Width * Height * Depth;


    // ----------< Constructor with validation >----------
    public Dimensions(
        decimal width,
        decimal height,
        decimal depth)
    {
        width.IsPositive(nameof(Width));
        height.IsPositive(nameof(Height));
        depth.IsPositive(nameof(Depth));

        Width = width;
        Height = height;
        Depth = depth;
    }
}