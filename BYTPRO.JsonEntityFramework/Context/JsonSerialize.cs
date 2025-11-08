using System.Collections;
using System.Text;
using System.Text.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public static class JsonSerialize
{
    public static string GetJson(this IEnumerable collection)
    {
        var asJson = new StringBuilder();
        asJson.Append('{');
        foreach (var ent in collection)
        {
            asJson.Append(JsonSerializer.Serialize(ent));
            asJson.Append(",\n");
        }
        var len = ",\n".Length;
        asJson.Remove(asJson.Length - len, len);
        asJson.Append('}');
        return asJson.ToString();
    }
}