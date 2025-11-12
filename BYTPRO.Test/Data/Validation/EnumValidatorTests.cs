using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class EnumValidatorTests
{
    private enum TestStatus
    {
        Unknown = 0,
        A = 1,
        B = 2
    }

    // IsDefined();
    
    [Fact]
    public void TestEnumIsDefinedPassesForValidValue()
    {
        var value = TestStatus.A;
        value.IsDefined();
        Assert.True(true);
    }

    [Fact]
    public void TestEnumIsDefinedThrowsForInvalidValue()
    {
        var bad = (TestStatus)999;
        Assert.Throws<ValidationException>(() => bad.IsDefined());
    }
}