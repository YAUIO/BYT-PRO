using BYTPRO.Data.Validation;
using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Test.Data.Validation;

public class StringValidatorTests
{
    [Fact]
    public void TestIsNotNullOrEmptyReturnsTrueForNotNullOrEmpty()
    {
        const string str = "not-null";
        str.IsNotNullOrEmpty();
        Assert.True(true);
    }

    [Fact]
    public void TestIsNotNullOrEmptyThrowsForNull()
    {
        string? str = null;
        Assert.Throws<ValidationException>(() => str.IsNotNullOrEmpty());
    }

    [Fact]
    public void TestIsNotNullOrEmptyThrowsForEmpty()
    {
        const string str = "";
        Assert.Throws<ValidationException>(() => str.IsNotNullOrEmpty());
    }
}