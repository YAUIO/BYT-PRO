namespace BYTPRO.Data.Models;

public class Order
{
    public int Id { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string Status { get; set; } // ENUM LATER TODO
    
    public virtual Person Customer { get; set; }
}