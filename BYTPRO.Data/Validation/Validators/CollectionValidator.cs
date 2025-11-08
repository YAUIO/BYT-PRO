namespace BYTPRO.Data.Validation.Validators;

using System.Collections.Generic;
using System.Linq;

public static class CollectionValidator
{
    public static void NotNullOrEmpty<T>(IEnumerable<T>? collection, string fieldName)
    {
        if (collection == null || !collection.Any())
            throw new ValidationException($"{fieldName} cannot be null or empty.");
    }

    public static void AllStringsNotNullOrEmpty(IEnumerable<string?>? collection, string fieldName)
    {
        NotNullOrEmpty(collection, fieldName);

        IList<string?> list = collection as IList<string?> ?? collection.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            StringValidator.NotNullOrEmpty(list[i], $"{fieldName}[{i}]");
        }
    }
}