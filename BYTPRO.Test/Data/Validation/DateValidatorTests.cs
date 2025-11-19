using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class DateValidatorTests
{
    // IsNotDefault();

    [Fact]
    public void TestIsNotDefaultPassesForNow()
    {
        var dt = DateTime.UtcNow;
        dt.IsNotDefault();
        Assert.True(true);
    }

    [Fact]
    public void TestIsNotDefaultThrowsForDefault()
    {
        var dt = default(DateTime);
        Assert.Throws<ValidationException>(() => dt.IsNotDefault());
    }

    // IsAfter();

    [Fact]
    public void TestIsAfterPassesWhenAfter()
    {
        var earlier = DateTime.UtcNow.AddMinutes(-2);
        var later = DateTime.UtcNow.AddMinutes(-1);

        later.IsAfter(earlier);
        Assert.True(true);
    }

    [Fact]
    public void TestIsAfterThrowsWhenNotAfter()
    {
        var a = DateTime.UtcNow;
        var b = a; // равно, end <= start
        Assert.Throws<ValidationException>(() => b.IsAfter(a));
    }

    // IsBefore();

    [Fact]
    public void TestIsBeforePassesWhenBefore()
    {
        var earlier = DateTime.UtcNow.AddMinutes(-2);
        var later = DateTime.UtcNow.AddMinutes(-1);

        earlier.IsBefore(later);
        Assert.True(true);
    }

    [Fact]
    public void TestIsBeforeThrowsWhenNotBefore()
    {
        var earlier = DateTime.UtcNow.AddMinutes(-2);
        var later = DateTime.UtcNow.AddMinutes(-1);

        Assert.Throws<ValidationException>(() => later.IsBefore(earlier));
    }
}