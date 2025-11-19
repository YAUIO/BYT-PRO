namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContextBuilder
{
    private HashSet<JsonEntityConfiguration> RegisteredEntities { get; } = [];
    
    private DirectoryInfo RootDir { get; set; }

    public JsonEntityBuilder<TJEntity> AddJsonEntity<TJEntity>()
    {
        return new JsonEntityBuilder<TJEntity>(this);
    }

    public JsonContextBuilder AddJsonEntity(JsonEntityConfiguration entityConfiguration)
    {
        RegisteredEntities.Add(entityConfiguration);
        return this;
    }

    public JsonContextBuilder WithRoot(DirectoryInfo rootDir)
    {
        RootDir = rootDir;
        return this;
    }

    public JsonContext Build()
    {
        if (RootDir == null) throw new ArgumentNullException(nameof(RootDir), "Provide WithRoot when creating JsonContext");
        return new JsonContext(RegisteredEntities, RootDir);
    }
}