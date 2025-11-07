using System.Text;
using System.Text.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    public HashSet<JsonEntitySet> tables { get; private set; }

    private DirectoryInfo Root { get; }
    
    public JsonContext(HashSet<JsonEntityConfiguration> entities, DirectoryInfo root)
    {
        Root = root;
        tables = [];

        if (!Root.Exists)
        {
            throw new FileNotFoundException("Root directory not found");
        }
        
        foreach (var ent in entities)
        {
            var path = Root.FullName + $"/{ent.FileName ?? ent.Target.Name}.json";
            
            var set = new JsonEntitySet(ent, path);
            tables.Add(set);
            
            if (!File.Exists(path))
            {
                File.Create(path);
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
        foreach (var json in tables)
        {
            if (!json.IsNeedsSaving())
            {
                continue;
            }

            var asJson = new StringBuilder();
            asJson.Append('{');
            foreach (var ent in json.Table)
            {
                asJson.Append(JsonSerializer.Serialize(ent));
                asJson.Append(",\n");
            }
            var len = ",\n".Length;
            asJson.Remove(asJson.Length - len, len);
            asJson.Append('}');
            
            File.Delete(json.Path);
            await File.WriteAllTextAsync(json.Path, asJson.ToString());

            json.MarkSaved();
        }
    }

    public DynamicSet<T> GetTable<T>()
    {
        return new (
            tables.Where(j => j.Type == typeof(T))
            .Select(j => j.Table)
            .Single()
        );
    }
}