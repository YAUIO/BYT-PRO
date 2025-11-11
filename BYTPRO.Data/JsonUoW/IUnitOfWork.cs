using BYTPRO.Data.Models.Orders;
using BYTPRO.Data.Models.People;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public interface IUnitOfWork
{
    public JsonEntitySet<Customer> Customers { get; }
    
    public JsonEntitySet<Order> Orders { get; }

    public Task SaveChangesAsync();

    public void SaveChanges();
    
    public Task RollbackAsync();

    public void Rollback();
}