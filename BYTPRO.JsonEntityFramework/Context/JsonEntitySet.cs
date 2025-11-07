namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntitySet
{
    public dynamic Table { get; private set; }

    private dynamic SavedTable { get; set; }
    
    public Type Type { get; }
    
    public string Path { get; }
    
    public JsonEntitySet(JsonEntityConfiguration config, string path)
    {
        Type = config.Target;
        Table = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(Type))
                ?? throw new InvalidOperationException($"Error while creating {config.Target.Name} table");
        SavedTable = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(Type))
                ?? throw new InvalidOperationException($"Error while creating {config.Target.Name} table");
        Path = path;
    }

    public void MarkSaved()
    {
        SavedTable = Enumerable.ToList(Table);
    }

    public bool IsNeedsSaving()
    {
        return !Table.SetEquals(SavedTable);
    }
}