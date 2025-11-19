using System.Collections;
using System.Reflection;
using IEnumerable = System.Collections.IEnumerable;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntityBuilder<TJEntity>(JsonContextBuilder contextBuilder)
{
    private JsonEntityConfiguration _entityConfiguration = new(typeof(TJEntity), null);

    public JsonEntityBuilder<TJEntity> WithFileName(string name)
    {
        _entityConfiguration = _entityConfiguration with { FileName = name };
        return this;
    }

    public JsonContextBuilder BuildEntity()
    {
        return contextBuilder.AddJsonEntity(_entityConfiguration);
    }
}