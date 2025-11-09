using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public static class JsonUnitOfWork
{
    private static readonly JsonContext Context = JsonContext.Context;
    
    public static JsonEntitySet<Person> Persons => Context.GetTable<Person>();

    public static JsonEntitySet<Order> Orders => Context.GetTable<Order>();

    public static async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
    
    public static void SaveChanges()
    {
        Context.SaveChanges();
    }
}