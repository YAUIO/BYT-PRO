namespace BYTPRO.Data.Validation.Validators;

using System.Collections.Generic;

public static class CollectionValidator
{
    public static void IsNotNullOrEmpty<T>(this ICollection<T>? collection, string fieldName = "ICollection")
    {
        if (collection == null || collection.Count == 0)
            throw new ValidationException($"{fieldName} cannot be null or empty.");
    }

    public static void AreAllStringsNotNullOrEmpty(this ICollection<string>? collection, string fieldName = "ICollection")
    {
        IsNotNullOrEmpty(collection, fieldName);

        var index = 0;
        foreach (var item in collection!)
        {
            item.IsNotNullOrEmpty($"{fieldName}[{index}]");
            index++;
        }
    }

    public static void AreAllElementsNotNull<T>(this ICollection<T>? collection, string fieldName = "ICollection")
    {
        IsNotNullOrEmpty(collection, fieldName);

        if (collection!.Any(item => item == null))
            throw new ValidationException($"{fieldName} contains a null element.");
    }
}