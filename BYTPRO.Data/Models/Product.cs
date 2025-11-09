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
    
    public List<string> Image { get; set; } = [];
    
    public decimal Weight { get; set; } = weight;
    
    public Dimensions Dimensions { get; set; } = dimensions;
    
    public List<BranchStock> StockLevels { get; set; } = [];

    public Product? ParentProduct { get; set; }
    
    public List<Product> ChildProducts { get; set; } = [];


    public override string ToString()
    {
        return $"Product: {Name}, Price: {Price}";
    }

    public void ViewDetails()
    {
        Console.WriteLine(this);
    }

    public static List<Product> SearchProducts(string query) // TODO move data storing to persistence, out of models
    {
        Console.WriteLine($"Searching products for: {query}");
        return [];
    }

    public static List<Product> ViewProductsByBranch(Branch branch) // TODO move data storing to persistence, out of models
    {
        Console.WriteLine($"Viewing products for branch: {branch.Name}");
        return [.. branch.Stock.Select(s => s.Product)];
    }
}