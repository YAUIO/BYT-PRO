namespace BYTPRO.Data.JsonUoW;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();

    public void SaveChanges();

    public Task RollbackAsync();

    public void Rollback();
}