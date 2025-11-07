using System.Collections;

namespace BYTPRO.JsonEntityFramework.Context;

public class DynamicSet<T> : IEnumerable<T>
{
    private readonly dynamic _set;

    private readonly HashSet<T> _castedSet = [];
    
    public DynamicSet(dynamic set)
    {
        _set = set;
        foreach (var v in set)
        {
            _castedSet.Add((T) v);
        }
    }
    
    public void Remove(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj), "Can't remove null object");
        _set.Remove(obj);
        _castedSet.Remove(obj);
    }

    public void Add(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj), "Can't add null object");
        _set.Add(obj);
        _castedSet.Add(obj);
    }
    
    public List<T> ToList()
    {
        return _castedSet.ToList();
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return _castedSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}