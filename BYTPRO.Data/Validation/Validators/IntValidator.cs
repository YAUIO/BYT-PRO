namespace BYTPRO.Data.Validation.Validators;

public static class IntValidator
{
    public static bool IsPositive(this int value, string fieldName = "int")
    {
        if (value <= 0)
            throw new ValidationException($"{fieldName} must be positive.");

        return true;
    }

    public static bool IsNonNegative(this int value, string fieldName = "int")
    {
        if (value < 0)
            throw new ValidationException($"{fieldName} cannot be negative.");

        return true;
    }

    public static bool IsInRange(this int value, int min, int max, string fieldName = "int")
    {
        if (value < min || value > max)
            throw new ValidationException($"{fieldName} must be between {min} and {max}.");

        return true;
    }
}