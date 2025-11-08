namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntityBuilder<TJEntity>(JsonContextBuilder contextBuilder)
{
    private JsonEntityConfiguration _entityConfiguration = new(typeof(TJEntity), null, [], []);

    public JsonEntityBuilder<TJEntity> WithFileName(string name)
    {
        _entityConfiguration = _entityConfiguration with { FileName = name };
        return this;
    }
    
    public JsonEntityBuilder<TJEntity> WithOne<TEntity>(string name)
    {
        _entityConfiguration.One.Add(new JsonConnection(name, typeof(TEntity)));
        _entityConfiguration = _entityConfiguration with { One = _entityConfiguration.One };
        return this;
    }
    
    public JsonEntityBuilder<TJEntity> WithMany<TEntity>(string name)
    {
        _entityConfiguration.Many.Add(new JsonConnection(name, typeof(List<TEntity>)));
        _entityConfiguration = _entityConfiguration with { Many = _entityConfiguration.Many };
        return this;
    }
    
    public JsonEntityBuilder<TJEntity> WithManyUnique<TEntity>(string name)
    {
        _entityConfiguration.Many.Add(new JsonConnection(name, typeof(HashSet<TEntity>)));
        _entityConfiguration = _entityConfiguration with { Many = _entityConfiguration.Many };
        return this;
    }

    public JsonContextBuilder BuildEntity()
    {
        return contextBuilder.AddJsonEntity(_entityConfiguration);
    }
}