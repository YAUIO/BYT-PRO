using System.Text.RegularExpressions;

namespace BYTPRO.Data.Validation.Validators;

public static partial class EmailValidator
{
    private static readonly Regex EmailRegex = CompileEmailRegex();

    public static void Validate(string email, string fieldName = "Email")
    {
        StringValidator.NotNullOrEmpty(email, fieldName);

        if (!EmailRegex.IsMatch(email))
            throw new ValidationException($"{fieldName} is not a valid email.");
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CompileEmailRegex();
}