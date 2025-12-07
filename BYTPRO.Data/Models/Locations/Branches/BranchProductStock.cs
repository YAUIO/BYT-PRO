using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations.Branches;

using Sales;

public class BranchProductStock
{
    #region ----------< Attributes >----------

    private int _quantity;

    #endregion

    #region ----------< Properties >----------

    public Branch Branch { get; }

    public Product Product { get; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            value.IsNonNegative(nameof(Quantity));
            _quantity = value;
        }
    }

    #endregion

    #region ----------< Constructor >----------

    public BranchProductStock(
        Branch branch,
        Product product,
        int quantity)
    {
        branch.IsNotNull(nameof(Branch));
        product.IsNotNull(nameof(Product));

        Branch = branch;
        Product = product;
        Quantity = quantity;
    }

    #endregion
}