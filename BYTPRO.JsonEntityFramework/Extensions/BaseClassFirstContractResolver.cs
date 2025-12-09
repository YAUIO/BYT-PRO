using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BYTPRO.JsonEntityFramework.Extensions;

public class BaseClassFirstContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        return base.CreateProperties(type, memberSerialization)
            .OrderBy(p => GetDepth(p.DeclaringType))
            .ToList();
    }

    private static int GetDepth(Type? t)
    {
        var depth = 0;

        while (t?.BaseType != null)
        {
            depth++;
            t = t.BaseType;
        }

        return depth;
    }
}