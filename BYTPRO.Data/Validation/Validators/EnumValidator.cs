namespace BYTPRO.Data.Validation.Validators;

public static class EnumValidator
{
    public static bool Defined<TEnum>(this TEnum value, string fieldName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
            throw new ValidationException($"{fieldName} has an invalid value.");
        
        return true;
    }
}