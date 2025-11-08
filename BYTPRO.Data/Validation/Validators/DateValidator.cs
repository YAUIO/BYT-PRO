namespace BYTPRO.Data.Validation.Validators;

public static class DateValidator
{
    public static void NotDefault(DateTime value, string fieldName)
    {
        if (value == default)
            throw new ValidationException($"{fieldName} must be a valid date.");
    }

    public static void StartBeforeEnd(DateTime start, DateTime end, string startName, string endName)
    {
        if (end < start)
            throw new ValidationException($"{endName} must be after {startName}.");
    }
}