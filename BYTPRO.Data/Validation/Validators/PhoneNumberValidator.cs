using System.Text.RegularExpressions;

namespace BYTPRO.Data.Validation.Validators;

public static partial class PhoneNumberValidator
{
    private static readonly Regex PhoneRegex = CreatePhoneRegex();

    public static void Validate(string phone, string fieldName = "Phone")
    {
        StringValidator.NotNullOrEmpty(phone, fieldName);

        if (!PhoneRegex.IsMatch(phone))
            throw new ValidationException($"{fieldName} must be a valid international phone number (E.164 format).");
    }

    [GeneratedRegex(@"^\+[1-9]\d{1,14}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreatePhoneRegex();
}