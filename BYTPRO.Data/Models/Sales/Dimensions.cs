using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Sales;

public record Dimensions
{
    #region ----------< Properties >----------

    public decimal Width { get; }
    public decimal Height { get; }
    public decimal Depth { get; }

    #endregion

    #region ----------< Calculated Properties >----------

    [JsonIgnore] public decimal Volume => Width * Height * Depth;

    #endregion

    #region ----------< Constructor with validation >----------

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

    #endregion
}