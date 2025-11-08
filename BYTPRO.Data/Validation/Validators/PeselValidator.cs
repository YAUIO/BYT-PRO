using System.Text.RegularExpressions;

namespace BYTPRO.Data.Validation.Validators;

public static partial class PeselValidator
{
    private static readonly Regex PeselRegex = CreatePeselRegex();

    public static void Validate(string pesel, string fieldName = "PESEL")
    {
        StringValidator.NotNullOrEmpty(pesel, fieldName);

        if (!PeselRegex.IsMatch(pesel))
            throw new ValidationException($"{fieldName} must consist of exactly 11 digits.");
    }

    [GeneratedRegex(@"^\d{11}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreatePeselRegex();
}