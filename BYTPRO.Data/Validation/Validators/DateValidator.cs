namespace BYTPRO.Data.Validation.Validators;

public static class DateValidator
{
    public static void IsNotDefault(this DateTime value, string fieldName = "DateTime")
    {
        if (value == default)
            throw new ValidationException($"{fieldName} must be a valid date.");
    }

    public static void IsBefore(this DateTime start, DateTime end, string startName = "Start", string endName = "End")
    {
        if (end < start)
            throw new ValidationException($"{endName} must be after {startName}.");
    }

    public static void IsAfter(this DateTime end, DateTime start, string endName = "End", string startName = "Start")
    {
        if (end <= start)
            throw new ValidationException($"{endName} must be after {startName}.");
    }
}