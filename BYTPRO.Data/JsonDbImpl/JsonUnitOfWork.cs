namespace BYTPRO.Data.JsonDbImpl;

public class JsonUnitOfWork : IUnitOfWork
{
    // IEnumerable<TEntity> Entity { get; private set; }


    public async Task SaveChangesAsync()
    {
        foreach (var prop in typeof(JsonUnitOfWork).GetProperties())
        {
            
        }
    }
}