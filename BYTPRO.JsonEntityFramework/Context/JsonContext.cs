using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using BYTPRO.JsonEntityFramework.Attributes;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    private HashSet<dynamic> Tables { get; set; }

    private DirectoryInfo Root { get; }

    public JsonContext(HashSet<JsonEntityConfiguration> entities, DirectoryInfo root)
    {
        Root = root;
        Tables = [];

        Root.Create();

        if (!Root.Exists)
        {
            throw new FileNotFoundException("Root directory not found");
        }

        foreach (var ent in entities)
        {
            var path = Root.FullName + $"/{ent.FileName ?? ent.Target.Name}.json";

            dynamic set = Activator.CreateInstance(typeof(JsonEntitySet<>).MakeGenericType(ent.Target), [ent, path]);
            Tables.Add(set ?? throw new InvalidOperationException("Null table created"));

            if (!File.Exists(path))
            {
                var stream = File.Create(path);
                stream.Write("[]"u8);
                stream.Close();
            }
            else
            {
                // Write loading logic
                using var fileStream = File.Open(path, FileMode.Open);

                var enumerable = (JsonElement)(JsonSerializer.Deserialize<dynamic>(fileStream, JsonSerializerExtensions.Options)
                                               ?? throw new InvalidCastException("Can't be a null JsonElement"));

                foreach (var obj in enumerable.EnumerateArray().Select(j => j.Deserialize(ent.Target, JsonSerializerOptions.Default)))
                {
                    
                    dynamic casted = obj;
                    if (!casted.GetType().IsDefined(typeof(HasExtentAttribute), false))
                    {
                        set.Add(casted);
                    }
                }
            }
        }
    }

    public JsonEntitySet<T> GetTable<T>()
    {
        return Tables.Single(j => j.GetType() == typeof(T));
    }

    public async Task SaveChangesAsync()
    {
        foreach (var json in Tables.Where(json => !json.IsSaved()))
        {
            await File.WriteAllTextAsync(json.Path, JsonSerializerExtensions.ToJson(json));
            json.MarkSaved();
        }
    }

    public void SaveChanges()
    {
        foreach (var json in Tables.Where(json => !json.IsSaved()))
        {
            File.WriteAllText(json.Path, JsonSerializerExtensions.ToJson(json));
            json.MarkSaved();
        }
    }
}