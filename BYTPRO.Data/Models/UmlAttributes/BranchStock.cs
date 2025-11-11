using BYTPRO.Data.Models.Branches;
using BYTPRO.Data.Models.Sales;

namespace BYTPRO.Data.Models.UmlAttributes;

public record BranchStock(Branch Branch, Product Product, int AvailableQuantity);