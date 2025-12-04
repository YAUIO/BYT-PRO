using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BYTPRO.JsonEntityFramework.Extensions;

public class ParentFirstContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var props = base.CreateProperties(type, memberSerialization);

        // Sort parent properties before child properties
        return props
            .OrderBy(p => GetDepth(p.DeclaringType)) // Base class depth = 0, child = 1, grandchild = 2...
            .ThenBy(p => p.Order ?? 0)
            .ThenBy(p => p.PropertyName)
            .ToList();
    }

    private static int GetDepth(Type? t)
    {
        if (t == null) return 0;

        var depth = 0;
        while (t.BaseType != null)
        {
            depth++;
            t = t.BaseType;
        }

        return depth;
    }
}