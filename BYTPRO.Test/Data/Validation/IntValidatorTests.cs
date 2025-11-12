using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Test.Data.Validation;

public class IntValidatorTests
{
    // IsPositive();
    [Fact]
    public void TestIsPositiveForPositive()
    {
        var value = 1;
        value.IsPositive();
        Assert.True(true);
    }

    [Fact]
    public void TestIsPositiveForNegative()
    {
        var value = -1;
        Assert.Throws<ValidationException>(() => value.IsPositive());
    }

    [Fact]
    public void TestIsPositiveForZero()
    {
        var value = 0;
        Assert.Throws<ValidationException>(() => value.IsPositive());
    }
    
    // IsNonNegative();
    [Fact]
    public void TestIsNonNegativeForPositive()
    {
        var value = 1;
        value.IsNonNegative();
        Assert.True(true);
    }

    [Fact]
    public void TestIsNonNegativeForNegative()
    {
        var value = -1;
        Assert.Throws<ValidationException>(() => value.IsNonNegative());
    }

    [Fact]
    public void TestIsNonNegativeForZero()
    {
        var value = 0;
        value.IsNonNegative();
        Assert.True(true);
    }
}