using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

using Sales;

public class BranchProductStock
{
    // ----------< Attributes >----------
    private int _quantity;


    // ----------< Properties >----------
    public Branch Branch { get; }

    public Product Product { get; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            value.IsNonNegative(nameof(Quantity)); // Allow setting to 0 items
            _quantity = value;
        }
    }


    // ----------< Constructor >----------
    public BranchProductStock(
        Branch branch,
        Product product,
        int quantity)
    {
        branch.IsNotNull(nameof(Branch));
        product.IsNotNull(nameof(Product));
        quantity.IsPositive(nameof(Quantity)); // Require at least 1 item to create

        Branch = branch;
        Product = product;
        Quantity = quantity;
    }
}