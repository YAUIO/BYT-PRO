namespace BYTPRO.Data.Validation.Validators;

public static class ObjectValidator
{
    public static bool NotNull(this object? value, string fieldName)
    {
        if (value == null)
            throw new ValidationException($"{fieldName} cannot be null.");
        
        return true;
    }
}