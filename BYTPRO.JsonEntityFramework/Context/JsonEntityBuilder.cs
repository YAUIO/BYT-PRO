using System.Collections;
using System.Reflection;
using IEnumerable = System.Collections.IEnumerable;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntityBuilder<TJEntity>(JsonContextBuilder contextBuilder)
{
    private JsonEntityConfiguration _entityConfiguration = new(typeof(TJEntity), null, [], []);

    public JsonEntityBuilder<TJEntity> WithFileName(string name)
    {
        _entityConfiguration = _entityConfiguration with { FileName = name };
        return this;
    }
    
    public JsonEntityBuilder<TJEntity> WithOne<TEntity>(PropertyInfo property)
    {
        throw new NotSupportedException("Not needed yet");
        
        if (property.PropertyType != typeof(TEntity))
            throw new InvalidOperationException($"{property.PropertyType} property is invalid type");
        
        _entityConfiguration.One.Add(property);
        _entityConfiguration = _entityConfiguration with { One = _entityConfiguration.One };
        return this;
    }
    
    public JsonEntityBuilder<TJEntity> WithMany<TEntity>(PropertyInfo property)
    {
        throw new NotSupportedException("Not needed yet");
        
        if (!property.PropertyType.IsAssignableTo(typeof(IEnumerable)))
            throw new InvalidOperationException($"{property.PropertyType} property is not a collection");
        
        if (property.PropertyType.GetGenericArguments()[0] != typeof(TEntity))
            throw new InvalidOperationException($"{property.PropertyType} property is invalid generic type");
        
        _entityConfiguration.Many.Add(property);
        _entityConfiguration = _entityConfiguration with { Many = _entityConfiguration.Many };
        return this;
    }

    public JsonContextBuilder BuildEntity()
    {
        return contextBuilder.AddJsonEntity(_entityConfiguration);
    }
}