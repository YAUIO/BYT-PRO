namespace BYTPRO.Data.Validation.Validators;

public static class StringValidator
{
    public static void IsNotNullOrEmpty(this string? value, string fieldName = "string")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} cannot be null or empty.");
    }

    public static void IsBelowMaxLength(this string? value, int maxLength, string fieldName = "string")
    {
        if (value?.Length > maxLength)
            throw new ValidationException($"{fieldName} cannot exceed {maxLength} characters.");
    }
}