namespace BYTPRO.Data.Validation.Validators;

public static class DecimalValidator
{
    public static void Positive(decimal value, string fieldName)
    {
        if (value <= 0)
            throw new ValidationException($"{fieldName} must be positive.");
    }

    public static void NonNegative(decimal value, string fieldName)
    {
        if (value < 0)
            throw new ValidationException($"{fieldName} cannot be negative.");
    }

    public static void InRange(decimal value, decimal min, decimal max, string fieldName)
    {
        if (value < min || value > max)
            throw new ValidationException($"{fieldName} must be between {min} and {max}.");
    }
}