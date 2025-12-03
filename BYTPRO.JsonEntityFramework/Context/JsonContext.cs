using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using BYTPRO.JsonEntityFramework.Extensions;
using Newtonsoft.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    public static int Contexts { get; private set; }
    public JsonContext(HashSet<JsonEntityConfiguration> entities, DirectoryInfo root)
    {
        Contexts++;
        
        Context ??= this;

        Root = root;
        Tables = [];

        Root.Create();

        if (!Root.Exists)
            throw new FileNotFoundException("Root directory not found");


        foreach (var ent in entities)
        {
            var path = Path.Combine(Root.FullName, $"{ent.FileName ?? ent.Target.Name}.json");

            dynamic set = Activator.CreateInstance(typeof(JsonEntitySet<>).MakeGenericType(ent.Target), ent, path);
            Tables.Add(set ?? throw new InvalidOperationException("Null table created"));

            if (!File.Exists(path))
            {
                var stream = File.Create(path);
                stream.Close();
            }
            else
            {
                var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

                fileLock.Wait();

                try
                {
                    using var fileStream = File.Open(path, FileMode.Open);
                    using var reader = new StreamReader(fileStream);
                    
                    var json = reader.ReadToEnd();

                    var list = JsonConvert.DeserializeObject(
                        json,
                        typeof(List<>).MakeGenericType(ent.Target),
                        JsonSerializerExtensions.Options
                    );
                    
                    if (list != null)
                        foreach (var item in (IEnumerable)list)
                        {
                            dynamic casted = item;
                            set.Add(casted);
                        }
                }
                finally
                {
                    fileLock.Release();
                }
            }
        }
    }

    public static JsonContext? Context { get; private set; }


    private HashSet<dynamic> Tables { get; }

    private DirectoryInfo Root { get; }

    private ConcurrentDictionary<string, SemaphoreSlim> FileLocks { get; } = new();

    public static void SetContext(JsonContext context)
    {
        Context = context;
    }

    public JsonEntitySet<T> GetTable<T>()
    {
        return Tables.Single(j => j.GetGenericType() == typeof(T));
    }

    public async Task SaveChangesAsync()
    {
        foreach (var json in Tables.Where(json => !json.IsSaved()))
        {
            if (json.Path is not string path)
            {
                throw new InvalidCastException("Path can't be null");
            }

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            await fileLock.WaitAsync();
            try
            {
                await File.WriteAllTextAsync(path, JsonSerializerExtensions.ToJson(json), Encoding.UTF8);
                json.MarkSaved();
            }
            finally
            {
                fileLock.Release();
            }
        }
    }

    public void SaveChanges()
    {
        foreach (var json in Tables.Where(json => !json.IsSaved()))
        {
            if (json.Path is not string path)
            {
                throw new InvalidCastException("Path can't be null");
            }

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            fileLock.Wait();
            try
            {
                File.WriteAllText(path, JsonSerializerExtensions.ToJson(json), Encoding.UTF8);
                json.MarkSaved();
            }
            finally
            {
                fileLock.Release();
            }
        }
    }

    public async Task RollbackAsync()
    {
        foreach (var table in Tables.Where(t => !t.IsSaved()))
        {
            var path = (string)table.Path;

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            await fileLock.WaitAsync();

            try
            {
                await using var fileStream = File.Open(path, FileMode.Open);
                using var reader = new StreamReader(fileStream);
                    
                var json = await reader.ReadToEndAsync();

                table.Clear();
                
                var type = (Type)table.GetGenericType();

                var list = JsonConvert.DeserializeObject(
                    json,
                    typeof(List<>).MakeGenericType(type),
                    JsonSerializerExtensions.Options
                );
                
                if (list != null)
                    foreach (var item in (IEnumerable)list)
                    {   
                        dynamic casted = item;
                        table.Add(casted);
                    }

                table.MarkSaved();

                fileStream.Close();
            }
            finally
            {
                fileLock.Release();
            }
        }
    }

    public void Rollback()
    {
        foreach (var table in Tables.Where(t => !t.IsSaved()))
        {
            var path = (string)table.Path;

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            fileLock.Wait();

            try
            {
                using var fileStream = File.Open(path, FileMode.Open);
                using var reader = new StreamReader(fileStream);
                    
                var json = reader.ReadToEnd();

                table.Clear();

                var type = (Type)table.GetGenericType();

                var list = JsonConvert.DeserializeObject(
                    json,
                    typeof(List<>).MakeGenericType(type),
                    JsonSerializerExtensions.Options
                );
                
                if (list != null)
                    foreach (var item in (IEnumerable)list)
                    {
                        dynamic casted = item;
                        table.Add(casted);
                    }

                table.MarkSaved();

                fileStream.Close();
            }
            finally
            {
                fileLock.Release();
            }
        }
    }
}