namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContextBuilder
{
    private HashSet<JsonEntityConfiguration> RegisteredEntities { get; } = [];

    public JsonContextBuilder AddJsonEntity<TJEntity>()
    {
        RegisteredEntities.Add(new JsonEntityConfiguration(typeof(TJEntity)));
        return this;
    }

    public JsonContext BuildWithDbRoot(string dbRoot)
    {
        return new JsonContext(RegisteredEntities, new FileInfo(dbRoot));
    }
}