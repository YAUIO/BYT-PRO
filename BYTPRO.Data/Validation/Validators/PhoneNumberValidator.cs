using System.Text.RegularExpressions;

namespace BYTPRO.Data.Validation.Validators;

public static partial class PhoneNumberValidator
{
    private static readonly Regex PhoneRegex = CreatePhoneRegex();

    public static bool IsPhoneNumber(this string phone, string fieldName = "Phone")
    {
        phone.IsNotNullOrEmpty(fieldName);

        if (!PhoneRegex.IsMatch(phone))
            throw new ValidationException($"{fieldName} must be a valid international phone number (E.164 format).");
        
        return true;
    }

    [GeneratedRegex(@"^\+[1-9]\d{1,14}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreatePhoneRegex();
}