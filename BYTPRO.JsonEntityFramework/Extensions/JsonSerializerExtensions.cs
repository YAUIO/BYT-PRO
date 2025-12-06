using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class JsonSerializerExtensions
{
    internal static readonly JsonSerializerSettings Options = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        ContractResolver = new ParentFirstContractResolver()
    };

    static JsonSerializerExtensions()
    {
        Options.Converters.Add(new StringEnumConverter());
    }

    public static string ToJson(this IEnumerable collection)
    {
        return JsonConvert.SerializeObject(collection, Options);
    }
}