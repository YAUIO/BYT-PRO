using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using BYTPRO.JsonEntityFramework.Extensions;
using Newtonsoft.Json;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    private static int Contexts { get; set; }

    public JsonContext(HashSet<JsonEntityConfiguration> entities, FileInfo dbFile)
    {
        Contexts++;

        Context ??= this;

        DbFile = dbFile;
        Tables = [];

        if (!DbFile.Exists)
        {
            DbFile.Create();
        }

        if (!DbFile.Exists)
        {
            throw new FileNotFoundException("Db file not found and/or couldn't be created");
        }

        DbPath = $"{DbFile.FullName}{(dbFile.Name.EndsWith(".json", StringComparison.CurrentCulture) ? "" : ".json")}";

        foreach (var ent in entities)
        {
            Tables.Add(Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(ent.Target))!);
        }
        
        if (!File.Exists(DbPath))
        {
            return;
        }

        DbLock.Wait();

        try
        {
            using var fileStream = File.Open(DbPath, FileMode.Open);
            using var reader = new StreamReader(fileStream);

            var json = reader.ReadToEnd();

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(),JsonSerializerExtensions.Options);

            Tables = db!;

            if (Tables.Count != entities.Count)
                throw new InvalidDataException("Invalid DB configuration");
        }
        finally
        {
            DbLock.Release();
        }
    }

    public static JsonContext? Context { get; private set; }

    private HashSet<dynamic> Tables { get; set; }

    private FileInfo DbFile { get; }
    
    public string DbPath { get; }

    private SemaphoreSlim DbLock { get; } = new(1, 1);

    public static void SetContext(JsonContext context)
    {
        Context = context;
    }

    public HashSet<T> GetTable<T>()
    {
        return Tables.Single(j => j.GetType().GenericTypeArguments[0] == typeof(T));
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

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(),JsonSerializerExtensions.Options);

            Tables = db!;
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

            dynamic db = JsonConvert.DeserializeObject(json, Tables.GetType(),JsonSerializerExtensions.Options);
            
            Tables = db!;
        }
        finally
        {
            DbLock.Release();
        }
    }
}