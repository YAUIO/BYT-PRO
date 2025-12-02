using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

using Sales;

public class BranchProductStock
{
    // ----------< Attributes >----------
    private int _quantity;

    // ----------< Associations >----------
    public Branch Branch { get; }
    public Product Product { get; }

    // ----------< Properties >----------
    public int Quantity
    {
        get => _quantity;
        set
        {
            value.IsNonNegative(nameof(Quantity));
            _quantity = value;
        }
    }

    // ----------< Constructor >----------
    public BranchProductStock(Branch branch, Product product, int quantity)
    {
        branch.IsNotNull(nameof(branch));
        product.IsNotNull(nameof(product));
        quantity.IsNonNegative(nameof(quantity));

        Branch = branch;
        Product = product;
        Quantity = quantity;

        Branch.AddStock(this);
        Product.AddStock(this);
    }

    public void Dissolve()
    {
        Branch.RemoveStock(this);
        Product.RemoveStock(this);
    }
}