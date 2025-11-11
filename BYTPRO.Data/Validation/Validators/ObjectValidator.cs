namespace BYTPRO.Data.Validation.Validators;

public static class ObjectValidator
{
    public static void IsNotNull(this object? value, string fieldName = "object")
    {
        if (value == null)
            throw new ValidationException($"{fieldName} cannot be null.");
    }
}