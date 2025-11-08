using BYTPRO.Data.Models.Attributes;
using BYTPRO.Data.Models.Branches;

namespace BYTPRO.Data.Models;

public class Product(
    string name,
    decimal price,
    decimal weight,
    Dimensions dimensions,
    string description)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal Price { get; set; } = price;
    public List<string> Image { get; set; } = new List<string>();
    public decimal Weight { get; set; } = weight;
    public Dimensions Dimensions { get; set; } = dimensions;
    
    public List<BranchStock> StockLevels { get; set; } = new List<BranchStock>();

    public Product? ParentProduct { get; set; }
    public List<Product> ChildProducts { get; set; } = new List<Product>();


    public void ViewDetails()
    {
        Console.WriteLine($"Product: {Name}, Price: {Price}");
    }

    public static List<Product> SearchProducts(string query)
    {
        Console.WriteLine($"Searching products for: {query}");
        return new List<Product>();
    }

    public static List<Product> ViewProductsByBranch(Branch branch)
    {
        Console.WriteLine($"Viewing products for branch: {branch.Name}");
        return branch.Stock.Select(s => s.Product).ToList();
    }
}