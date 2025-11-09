using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public class JsonUnitOfWork(JsonContext context) : IUnitOfWork
{
    public JsonEntitySet<Person> Persons => context.GetTable<Person>();

    public JsonEntitySet<Order> Orders => context.GetTable<Order>();

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    
    public void SaveChanges()
    {
        context.SaveChanges();
    }
}