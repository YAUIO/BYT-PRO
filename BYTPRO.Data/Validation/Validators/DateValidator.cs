namespace BYTPRO.Data.Validation.Validators;

public static class DateValidator
{
    public static bool NotDefault(this DateTime value, string fieldName)
    {
        if (value == default)
            throw new ValidationException($"{fieldName} must be a valid date.");
        
        return true;
    }

    public static bool Before(this DateTime start, DateTime end, string startName, string endName)
    {
        if (end < start)
            throw new ValidationException($"{endName} must be after {startName}.");
        
        return true;
    }
}