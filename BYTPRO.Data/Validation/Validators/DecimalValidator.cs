namespace BYTPRO.Data.Validation.Validators;

public static class DecimalValidator
{
    public static void IsPositive(this decimal value, string fieldName = "decimal")
    {
        if (value <= 0)
            throw new ValidationException($"{fieldName} must be positive.");
    }

    public static void IsNonNegative(this decimal value, string fieldName = "decimal")
    {
        if (value < 0)
            throw new ValidationException($"{fieldName} cannot be negative.");
    }
}