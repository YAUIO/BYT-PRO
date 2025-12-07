namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContextBuilder
{
    private HashSet<JsonEntityConfiguration> RegisteredEntities { get; } = [];

    private FileInfo DbFile { get; set; }

    public JsonContextBuilder AddJsonEntity<TJEntity>()
    {
        RegisteredEntities.Add(new JsonEntityConfiguration(typeof(TJEntity)));
        return this;
    }

    public JsonContextBuilder AddJsonEntity(JsonEntityConfiguration entityConfiguration)
    {
        RegisteredEntities.Add(entityConfiguration);
        return this;
    }

    public JsonContextBuilder WithDbFile(FileInfo dbFile)
    {
        DbFile = dbFile;
        return this;
    }

    public JsonContext Build()
    {
        return DbFile == null
            ? throw new ArgumentNullException(nameof(DbFile), "Provide WithRoot when creating JsonContext")
            : new JsonContext(RegisteredEntities, DbFile);
    }
}