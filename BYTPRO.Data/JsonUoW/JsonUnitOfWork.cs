using BYTPRO.Data.Models;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public class JsonUnitOfWork(JsonContext context) : IUnitOfWork
{
    public JsonEntitySet<Person> Persons => context.GetTable<Person>();

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}