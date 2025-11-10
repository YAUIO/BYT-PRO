using BYTPRO.Data.Models.Branches;

namespace BYTPRO.Data.Models.UmlAttributes;

public record BranchStock(Branch Branch, Product Product, int AvailableQuantity);