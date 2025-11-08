using BYTPRO.Data.Models.Branches;

namespace BYTPRO.Data.Models;

public class BranchStock(Branch branch, Product product, int availableQuantity)
{
    public Branch Branch { get; set; } = branch;
    public Product Product { get; set; } = product;
    public int AvailableQuantity { get; set; } = availableQuantity;
}