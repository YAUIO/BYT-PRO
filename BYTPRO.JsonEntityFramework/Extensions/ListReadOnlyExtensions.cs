using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class ListReadOnlyExtensions
{
    public static DeserializableReadOnlyList<T> ToDeserializableReadOnlyList<T>(this IList<T> collection)
    {
        return new DeserializableReadOnlyList<T>(collection.AsReadOnly());
    }
}