using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class DecimalValidatorTests
{
    // IsPositive();

    [Fact]
    public void TestIsPositivePassesForPositive()
    {
        const decimal value = 0.01m;
        value.IsPositive();
        Assert.True(true);
    }

    [Fact]
    public void TestIsPositiveThrowsForZero()
    {
        const decimal value = 0m;
        Assert.Throws<ValidationException>(() => value.IsPositive());
    }

    [Fact]
    public void TestIsPositiveThrowsForNegative()
    {
        const decimal value = -0.01m;
        Assert.Throws<ValidationException>(() => value.IsPositive());
    }

    // IsNonNegative();

    [Fact]
    public void TestIsNonNegativePassesForZero()
    {
        const decimal value = 0m;
        value.IsNonNegative();
        Assert.True(true);
    }

    [Fact]
    public void TestIsNonNegativeThrowsForNegative()
    {
        const decimal value = -0.01m;
        Assert.Throws<ValidationException>(() => value.IsNonNegative());
    }
}