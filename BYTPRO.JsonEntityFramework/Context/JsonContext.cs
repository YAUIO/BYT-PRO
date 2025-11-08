using System.Text;
using System.Text.Json;
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

        if (!Root.Exists)
        {
            throw new FileNotFoundException("Root directory not found");
        }
        
        foreach (var ent in entities)
        {
            var path = Root.FullName + $"/{ent.FileName ?? ent.Target.Name}.json";

            var set = Activator.CreateInstance(typeof(JsonEntitySet<>).MakeGenericType(ent.Target), args: [ent, path]);
            Tables.Add(set ?? throw new InvalidOperationException("Null table created"));
            
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            else
            {
                // Write loading logic
                // using var fileStream = File.Open(path, FileMode.Open);
                // set.Table.Add();
            }
        }
    }
    
    public async Task SaveChangesAsync()
    {
        foreach (var json in Tables.Where(json => !json.IsSaved()))
        {
            await File.WriteAllTextAsync(json.Path, JsonSerialize.GetJson(json));
            json.MarkSaved();
        }
    }

    public JsonEntitySet<T> GetTable<T>()
    {
        return Tables.Single(j => j.GetType() == typeof(T));
    }
}