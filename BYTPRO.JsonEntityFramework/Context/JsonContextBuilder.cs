namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContextBuilder
{
    private HashSet<JsonEntityConfiguration> RegisteredEntities { get; } = [];

    public JsonContextBuilder AddJsonEntity<TJEntity>()
    {
        RegisteredEntities.Add(new JsonEntityConfiguration(typeof(TJEntity)));
        return this;
    }

    public JsonContext BuildWithDbFile(FileInfo dbFile)
    {
        ArgumentNullException.ThrowIfNull(dbFile);
        return new JsonContext(RegisteredEntities, dbFile);
    }
}