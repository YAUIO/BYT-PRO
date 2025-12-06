using Newtonsoft.Json;

namespace BYTPRO.Data.Models.Sales;

public record ProductEntry(
    [JsonProperty(nameof(Product))] Product Product,
    [JsonProperty(nameof(Quantity))] int Quantity
);