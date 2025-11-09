using System.Collections;
using System.Text.Json;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class JsonSerializerExtensions
{
    internal static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public static string ToJson(this IEnumerable collection)
    {
        return JsonSerializer.Serialize(collection, Options);
    }
}