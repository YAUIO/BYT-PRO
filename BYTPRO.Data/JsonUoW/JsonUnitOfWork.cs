using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.Sales.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public class JsonUnitOfWork(JsonContext context) : IUnitOfWork
{

    public JsonEntitySet<Customer> Customers => context.GetTable<Customer>();
    public JsonEntitySet<Order> Orders => context.GetTable<Order>();

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    
    public void SaveChanges()
    {
        context.SaveChanges();
    }

    public async Task RollbackAsync()
    {
        await context.RollbackAsync();
    }

    public void Rollback()
    {
        context.Rollback();
    }
}