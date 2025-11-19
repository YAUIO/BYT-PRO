using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class ObjectValidatorTests
{
    // IsNotNull();

    [Fact]
    public void TestIsNotNullPassesForObject()
    {
        var obj = new { Id = 1 };
        obj.IsNotNull();
        Assert.True(true);
    }

    [Fact]
    public void TestIsNotNullThrowsForNull()
    {
        object? obj = null;
        Assert.Throws<ValidationException>(() => obj!.IsNotNull());
    }
}