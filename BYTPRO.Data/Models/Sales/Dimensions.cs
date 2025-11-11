using System.Text.Json.Serialization;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record Dimensions
{
    // ----------< Properties >----------
    public decimal Width { get; init; }
    public decimal Height { get; init; }
    public decimal Depth { get; init; }


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