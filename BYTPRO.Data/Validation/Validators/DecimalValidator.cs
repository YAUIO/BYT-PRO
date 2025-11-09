namespace BYTPRO.Data.Validation.Validators;

public static class DecimalValidator
{
    public static bool IsPositive(this decimal value, string fieldName)
    {
        if (value <= 0)
            throw new ValidationException($"{fieldName} must be positive.");
        
        return true;
    }

    public static bool IsNonNegative(this decimal value, string fieldName)
    {
        if (value < 0)
            throw new ValidationException($"{fieldName} cannot be negative.");
        
        return true;
    }

    public static bool IsInRange(this decimal value, decimal min, decimal max, string fieldName)
    {
        if (value < min || value > max)
            throw new ValidationException($"{fieldName} must be between {min} and {max}.");
        
        return true;
    }
}