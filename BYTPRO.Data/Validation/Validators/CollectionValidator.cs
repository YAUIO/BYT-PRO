namespace BYTPRO.Data.Validation.Validators;

using System.Collections.Generic;
using System.Linq;

public static class CollectionValidator
{
    public static bool NotNullOrEmpty<T>(this IEnumerable<T>? collection, string fieldName)
    {
        if (collection == null || !collection.Any())
            throw new ValidationException($"{fieldName} cannot be null or empty.");
        
        return true;
    }

    public static bool AllStringsNotNullOrEmpty(this IEnumerable<string?> collection, string fieldName)
    {
        var enumerable = collection.ToList();
        enumerable.NotNullOrEmpty(fieldName);

        var list = collection as IList<string?> ?? enumerable.ToList();

        for (var i = 0; i < list.Count; i++)
        {
            list[i].NotNullOrEmpty($"{fieldName}[{i}]");
        }
        
        return true;
    }
}