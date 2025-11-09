namespace BYTPRO.Data.Validation.Validators;

public static class StringValidator
{
    public static bool NotNullOrEmpty(this string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} cannot be null or empty.");

        return true;
    }

    public static bool MaxLength(this string? value, int maxLength, string fieldName)
    {
        if (value?.Length > maxLength)
            throw new ValidationException($"{fieldName} cannot exceed {maxLength} characters.");
        
        return true;
    }
}