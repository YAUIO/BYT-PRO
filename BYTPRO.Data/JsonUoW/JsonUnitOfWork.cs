using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public class JsonUnitOfWork(JsonContext context) : IUnitOfWork
{
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