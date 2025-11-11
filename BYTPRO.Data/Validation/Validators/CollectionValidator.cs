namespace BYTPRO.Data.Validation.Validators;

using System.Collections.Generic;
using System.Linq;

public static class CollectionValidator
{
    public static void IsNotNullOrEmpty<T>(this IEnumerable<T>? collection, string fieldName = "ICollection")
    {
        if (collection == null || !collection.Any())
            throw new ValidationException($"{fieldName} cannot be null or empty.");
    }

    public static void AreAllStringsNotNullOrEmpty(this IEnumerable<string?> collection, string fieldName = "ICollection")
    {
        var list = collection as IList<string?> ?? collection.ToList();

        for (var i = 0; i < list.Count; i++)
            list[i].IsNotNullOrEmpty($"{fieldName}[{i}]");
    }
}