using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BYTPRO.JsonEntityFramework.Context;

public class DeserializableReadOnlyList<T> : IList<T>
{
    [JsonInclude] private IList<T> _list = new List<T>();

    private bool _isConstructed;

    [JsonConstructor]
    public DeserializableReadOnlyList()
    {
    }

    public DeserializableReadOnlyList(ReadOnlyCollection<T> collection)
    {
        _list = collection;
        _isConstructed = true;
    }

    public void MakeReadOnly()
    {
        if (_isConstructed) return;
        _isConstructed = true;
        _list = _list.AsReadOnly();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public void Add(T item)
    {
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _list.Remove(item);
    }

    public int Count => _list.Count;

    public bool IsReadOnly => _list.IsReadOnly;

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}