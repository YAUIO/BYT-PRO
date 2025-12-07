using System.Collections.Concurrent;
using System.Text;
using BYTPRO.JsonEntityFramework.Extensions;
using Newtonsoft.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    public JsonContext(HashSet<JsonEntityConfiguration> entities, FileInfo dbFile)
    {
        Context ??= this;

        DbFile = dbFile;
        Entities = entities;
        Tables = new ConcurrentDictionary<Type, dynamic>();

        DbPath = $"{DbFile.FullName}{(dbFile.Name.EndsWith(".json", StringComparison.CurrentCulture) ? "" : ".json")}";

        foreach (var ent in entities.Select(e => e.Target))
        {
            var set = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent))!;
            if (!Tables.TryAdd(ent, set))
                Tables[ent] = set;
        }

        if (!File.Exists(DbPath))
        {
            var file = DbFile.Create();
            file.Close();
            return;
        }

        DbLock.Wait();

        try
        {
            using var fileStream = File.Open(DbPath, FileMode.Open);
            using var reader = new StreamReader(fileStream);

            var json = reader.ReadToEnd();

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(), JsonSerializerExtensions.Options);

            Tables = db ?? new ConcurrentDictionary<Type, dynamic>();

            foreach (var ent in entities.Select(e => e.Target).Where(e => !Tables.ContainsKey(e)))
            {
                Tables.TryAdd(ent, Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent))!);
            }
        }
        finally
        {
            DbLock.Release();
        }
    }

    public static JsonContext? Context { get; private set; }

    private ConcurrentDictionary<Type, dynamic> Tables { get; set; }

    private HashSet<JsonEntityConfiguration> Entities { get; }

    private FileInfo DbFile { get; }

    public string DbPath { get; }

    private SemaphoreSlim DbLock { get; } = new(1, 1);

    public static void SetContext(JsonContext context)
    {
        Context = context;
    }

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