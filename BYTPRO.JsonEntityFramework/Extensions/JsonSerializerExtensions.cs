using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class JsonSerializerExtensions
{
    internal static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string ToJson(this IEnumerable collection)
    {
        return JsonSerializer.Serialize(collection, Options);
    }
}