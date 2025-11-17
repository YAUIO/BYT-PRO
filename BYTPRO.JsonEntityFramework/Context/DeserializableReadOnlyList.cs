using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BYTPRO.JsonEntityFramework.Context;

public class DeserializableReadOnlyList<T> : IList<T>
{
    private ReadOnlyCollection<T> _collection;
    
    private List<T> _list = [];
    
    private bool _isConstructed;

    private IList<T> Active => _isConstructed ? _collection : _list;

    [JsonConstructor]
    public DeserializableReadOnlyList()
    {
        
    }

    public DeserializableReadOnlyList(ReadOnlyCollection<T> collection)
    {
        _collection = collection;
        _isConstructed = true;
    }

    public void MakeReadOnly()
    {
        _isConstructed = true;
        _collection = _list.AsReadOnly();
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return Active.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        Active.Add(item);
    }

    public void Clear()
    {
        Active.Clear();
    }

    public bool Contains(T item)
    {
        return Active.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Active.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return Active.Remove(item);
    }

    public int Count  => Active.Count;
    public bool IsReadOnly => Active.IsReadOnly;
    public int IndexOf(T item)
    {
        return Active.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        Active.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Active.RemoveAt(index);
    }

    public T this[int index]
    {
        get => Active[index];
        set => Active[index] = value;
    }
}