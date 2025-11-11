namespace BYTPRO.Data.Validation.Validators;

public static class IntValidator
{
    public static void IsPositive(this int value, string fieldName = "int")
    {
        if (value <= 0)
            throw new ValidationException($"{fieldName} must be positive.");
    }

    public static void IsNonNegative(this int value, string fieldName = "int")
    {
        if (value < 0)
            throw new ValidationException($"{fieldName} cannot be negative.");
    }
}