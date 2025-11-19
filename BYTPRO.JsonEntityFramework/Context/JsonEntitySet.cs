using System.Collections;

namespace BYTPRO.JsonEntityFramework.Context;

public class JsonEntitySet<TJEntity>(JsonEntityConfiguration config, string path) : ISet<TJEntity>
{
    private dynamic Table { get; } = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(typeof(TJEntity)))
                                     ?? throw new InvalidOperationException(
                                         $"Error while creating {config.Target.Name} table");

    public string Path { get; } = path;

    private bool _isSaved;

    public void MarkSaved()
    {
        _isSaved = true;
    }

    public bool IsSaved()
    {
        return _isSaved;
    }

    public Type GetGenericType()
    {
        return typeof(TJEntity);
    }

    public void Add(TJEntity item)
    {
        if (Table.Add(item)) _isSaved = false;
    }

    public IEnumerator<TJEntity> GetEnumerator()
    {
        return Table.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ICollection<TJEntity>.Add(TJEntity item)
    {
        if (Table.Add(item)) _isSaved = false;
    }

    public void ExceptWith(IEnumerable<TJEntity> other)
    {
        Table.ExceptWith(other);
    }

    public void IntersectWith(IEnumerable<TJEntity> other)
    {
        Table.IntersectWith(other);
    }

    public bool IsProperSubsetOf(IEnumerable<TJEntity> other)
    {
        return Table.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<TJEntity> other)
    {
        return Table.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<TJEntity> other)
    {
        return Table.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<TJEntity> other)
    {
        return Table.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<TJEntity> other)
    {
        return Table.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<TJEntity> other)
    {
        return Table.SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<TJEntity> other)
    {
        Table.SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<TJEntity> other)
    {
        Table.UnionWith(other);
    }

    bool ISet<TJEntity>.Add(TJEntity item)
    {
        bool added = Table.Add(item);
        if (added) _isSaved = false;
        return added;
    }

    public void Clear()
    {
        Table.Clear();
        _isSaved = false;
    }

    public bool Contains(TJEntity item)
    {
        return Table.Contains(item);
    }

    public void CopyTo(TJEntity[] array, int arrayIndex)
    {
        Table.CopyTo(array, arrayIndex);
    }

    public bool Remove(TJEntity item)
    {
        bool removed = Table.Remove(item);
        if (removed) _isSaved = false;
        return removed;
    }

    public int Count => Table.Count;

    public bool IsReadOnly => false;
}