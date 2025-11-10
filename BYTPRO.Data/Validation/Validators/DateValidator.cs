namespace BYTPRO.Data.Validation.Validators;

public static class DateValidator
{
    public static bool IsNotDefault(this DateTime value, string fieldName = "DateTime")
    {
        if (value == default)
            throw new ValidationException($"{fieldName} must be a valid date.");
        
        return true;
    }

    public static bool IsBefore(this DateTime start, DateTime end, string startName = "DateTime.Start", string endName = "DateTime.End")
    {
        if (end < start)
            throw new ValidationException($"{endName} must be after {startName}.");
        
        return true;
    }
    
    public static bool IsAfter(this DateTime end, DateTime start, string endName = "DateTime.End", string startName = "DateTime.Start")
    {
        if (end <= start)
            throw new ValidationException($"{endName} must be after {startName}.");
        
        return true;
    }
}