namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntityBuilder(Type entity, JsonContextBuilder contextBuilder)
{
    private JsonEntityConfiguration _entityConfiguration = new(entity, null, [], []);

    public JsonEntityBuilder WithFileName(string name)
    {
        _entityConfiguration = _entityConfiguration with { FileName = name };
        return this;
    }
    
    public JsonEntityBuilder WithOne<TJEntity>(string name)
    {
        _entityConfiguration.One.Add(new JsonConnection(name, typeof(TJEntity)));
        _entityConfiguration = _entityConfiguration with { One = _entityConfiguration.One };
        return this;
    }
    
    public JsonEntityBuilder WithMany<TJEntity>(string name)
    {
        _entityConfiguration.Many.Add(new JsonConnection(name, typeof(List<TJEntity>)));
        _entityConfiguration = _entityConfiguration with { Many = _entityConfiguration.Many };
        return this;
    }
    
    public JsonEntityBuilder WithManyUnique<TJEntity>(string name)
    {
        _entityConfiguration.Many.Add(new JsonConnection(name, typeof(HashSet<TJEntity>)));
        _entityConfiguration = _entityConfiguration with { Many = _entityConfiguration.Many };
        return this;
    }

    public JsonContextBuilder BuildEntity()
    {
        return contextBuilder.AddJsonEntity(_entityConfiguration);
    }
}