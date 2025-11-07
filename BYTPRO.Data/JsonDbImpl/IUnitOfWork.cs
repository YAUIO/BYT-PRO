namespace BYTPRO.Data.JsonDbImpl;

public interface IUnitOfWork
{
    // IEnumerable<TEntity> Entity { get; private set; }

    Task SaveChangesAsync();
}