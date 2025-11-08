namespace BYTPRO.Data.Validation.Validators;

public static class StringValidator
{
    public static void NotNullOrEmpty(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} cannot be null or empty.");
    }

    public static void MaxLength(string? value, int maxLength, string fieldName)
    {
        if (value?.Length > maxLength)
            throw new ValidationException($"{fieldName} cannot exceed {maxLength} characters.");
    }
}