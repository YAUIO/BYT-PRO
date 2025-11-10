using System.Collections.Concurrent;
using System.Text.Json;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonContext
{
    private HashSet<dynamic> Tables { get; set; }

    private DirectoryInfo Root { get; }

    private readonly object? _uow;

    public ConcurrentDictionary<string, SemaphoreSlim> FileLocks { get; } = new();

    public JsonContext(HashSet<JsonEntityConfiguration> entities, DirectoryInfo root, Type uow)
    {
        Root = root;
        Tables = [];

        Root.Create();

        if (!Root.Exists)
        {
            throw new FileNotFoundException("Root directory not found");
        }

        _uow = Activator.CreateInstance(uow, [this]);

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
                var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

                fileLock.Wait();

                try
                {
                    using var fileStream = File.Open(path, FileMode.Open);

                    var enumerable =
                        (JsonElement)(JsonSerializer.Deserialize<dynamic>(fileStream, JsonSerializerExtensions.Options)
                                      ?? throw new InvalidCastException("Can't be a null JsonElement"));

                    foreach (var obj in enumerable.EnumerateArray())
                    {
                        var result = obj.MapToEntity(ent.Target, _uow);

                        if (!result.GetType().GetProperties().Any(p => p.Name.Equals("Extent")))
                        {
                            dynamic casted = result;
                            set.Add(casted);
                        }
                    }
                }
                finally
                {
                    fileLock.Release();
                }

                SaveChanges();
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
            if (json.Path is not string path)
            {
                throw new InvalidCastException("Path can't be null");
            }

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            await fileLock.WaitAsync();
            try
            {
                await File.WriteAllTextAsync(path, JsonSerializerExtensions.ToJson(json));
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
                File.WriteAllText(path, JsonSerializerExtensions.ToJson(json));
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
        foreach (var table in Tables)
        {
            var path = (string)table.Path;

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            await fileLock.WaitAsync();

            try
            {
                await using var fileStream = File.Open(path, FileMode.Open);

                var enumerable =
                    (JsonElement)(await JsonSerializer.DeserializeAsync<dynamic>(fileStream,
                                      JsonSerializerExtensions.Options)
                                  ?? throw new InvalidCastException("Can't be a null JsonElement"));

                foreach (var obj in enumerable.EnumerateArray())
                {
                    var type = (Type)table.GetType().GetGenericArguments().Single();

                    var result = obj.MapToEntity(type, _uow);

                    if (!result.GetType().GetProperties().Any(p => p.Name.Equals("Extent")))
                    {
                        dynamic casted = result;
                        table.Add(casted);
                    }
                }

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
        foreach (var table in Tables)
        {
            var path = (string)table.Path;

            var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));

            fileLock.Wait();

            try
            {
                using var fileStream = File.Open(path, FileMode.Open);

                var enumerable =
                    (JsonElement)(JsonSerializer.Deserialize<dynamic>(fileStream, JsonSerializerExtensions.Options)
                                  ?? throw new InvalidCastException("Can't be a null JsonElement"));

                foreach (var obj in enumerable.EnumerateArray())
                {
                    var type = (Type)table.GetType().GetGenericArguments().Single();

                    var result = obj.MapToEntity(type, _uow);


                    if (!result.GetType().GetProperties().Any(p => p.Name.Equals("Extent")))
                    {
                        dynamic casted = result;
                        table.Add(casted);
                    }
                }

                fileStream.Close();
            }
            finally
            {
                fileLock.Release();
            }
        }
    }
}