using System.Collections.Concurrent;
using System.Text;
using BYTPRO.JsonEntityFramework.Extensions;
using Newtonsoft.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    public JsonContext(HashSet<JsonEntityConfiguration> entities, FileInfo dbFile)
    {
        Context ??= this; // Must be set early because deserialization may access JsonContext.Context.GetTable<T>()

        DbFile = dbFile;
        Entities = entities;
        DbPath = DbFile.FullName;

        // 1. Always create empty tables for all requested entity types
        Tables = new ConcurrentDictionary<Type, dynamic>();
        foreach (var ent in entities.Select(e => e.Target))
            Tables[ent] = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent))!;

        // 2. Ensure a file exists; if not, create an empty one and stop
        if (!File.Exists(DbPath))
        {
            Directory.CreateDirectory(DbFile.DirectoryName ?? Directory.GetCurrentDirectory());
            File.WriteAllText(DbPath, "");
            return;
        }

        // 3. Load + merge data
        DbLock.Wait();
        try
        {
            var json = File.ReadAllText(DbPath);
            if (string.IsNullOrWhiteSpace(json)) return;

            // 3. Deserialize into a temp object (it maybe null)
            var loaded = JsonConvert.DeserializeObject(
                json,
                Tables.GetType(),
                JsonSerializerExtensions.Options
            );

            if (loaded is not ConcurrentDictionary<Type, dynamic> loadedTables) return;

            // 4. Replace only the tables that were actually loaded
            foreach (var kvp in loadedTables)
                Tables[kvp.Key] = kvp.Value;
        }
        finally
        {
            DbLock.Release();
        }
    }

    public static JsonContext? Context { get; set; }

    private ConcurrentDictionary<Type, dynamic> Tables { get; set; }

    private HashSet<JsonEntityConfiguration> Entities { get; }

    private FileInfo DbFile { get; }

    public string DbPath { get; }

    private SemaphoreSlim DbLock { get; } = new(1, 1);

    public HashSet<T> GetTable<T>()
    {
        return Tables[typeof(T)];
    }

    public async Task SaveChangesAsync()
    {
        await DbLock.WaitAsync();
        try
        {
            await File.WriteAllTextAsync(DbPath, Tables.ToJson(), Encoding.UTF8);
        }
        finally
        {
            DbLock.Release();
        }
    }

    public void SaveChanges()
    {
        DbLock.Wait();
        try
        {
            File.WriteAllText(DbPath, Tables.ToJson(), Encoding.UTF8);
        }
        finally
        {
            DbLock.Release();
        }
    }

    public async Task RollbackAsync()
    {
        await DbLock.WaitAsync();

        try
        {
            await using var fileStream = File.Open(DbPath, FileMode.Open);
            using var reader = new StreamReader(fileStream);

            var json = await reader.ReadToEndAsync();

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(), JsonSerializerExtensions.Options);

            Tables = db!;

            foreach (var ent in Entities.Select(e => e.Target).Where(e => !Tables.ContainsKey(e)))
            {
                Tables.TryAdd(ent, Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent))!);
            }
        }
        finally
        {
            DbLock.Release();
        }
    }

    public void Rollback()
    {
        DbLock.Wait();

        try
        {
            using var fileStream = File.Open(DbPath, FileMode.Open);
            using var reader = new StreamReader(fileStream);

            var json = reader.ReadToEnd();

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(), JsonSerializerExtensions.Options);

            Tables = db!;

            foreach (var ent in Entities.Select(e => e.Target).Where(e => !Tables.ContainsKey(e)))
            {
                Tables.TryAdd(ent, Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent))!);
            }
        }
        finally
        {
            DbLock.Release();
        }
    }
}