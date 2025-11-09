using BYTPRO.Data.Models.Branches;

namespace BYTPRO.Data.Models.Attributes;

public record BranchStock(Branch Branch, Product Product, int AvailableQuantity);