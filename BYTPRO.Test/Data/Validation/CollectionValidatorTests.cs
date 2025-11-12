using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class CollectionValidatorTests
{
    // IsNotNullOrEmpty();

    [Fact]
    public void TestValidateCollectionForEmptyCollection()
    {
        var list = new List<int>();
        Assert.Throws<ValidationException>(() => list.IsNotNullOrEmpty());
    }

    [Fact]
    public void TestValidateCollectionForNullCollection()
    {
        List<int>? list = null;
        Assert.Throws<ValidationException>(() => list!.IsNotNullOrEmpty());
    }

    [Fact]
    public void TestValidateCollectionForNonEmptyCollection()
    {
        var list = new List<int> { 1, 2, 3 };
        var ex = Record.Exception(() => list.IsNotNullOrEmpty());
        Assert.Null(ex);
    }

    // AreAllStringsNotNullOrEmpty();
    [Fact]
    public void TestAllStringsNotNullOrEmptyForStringList()
    {
        var list = new List<string?> { "1", "2", "3" };
        var ex = Record.Exception(() => list.AreAllStringsNotNullOrEmpty());
        Assert.Null(ex);
    }

    [Fact]
    public void TestAllStringsNotNullOrEmptyForNullElement()
    {
        var list = new List<string?> { "1", null, "3" };
        Assert.Throws<ValidationException>(() => list.AreAllStringsNotNullOrEmpty());
    }

    [Fact]
    public void TestAllStringsNotNullOrEmptyForEmptyElement()
    {
        var list = new List<string?> { "1", "", string.Empty };
        Assert.Throws<ValidationException>(() => list.AreAllStringsNotNullOrEmpty());
    }
    
    [Fact]
    public void TestAllStringsNotNullOrEmptyForWhitespaceElement()
    {
    var list = new List<string?> { "ok", "   " };
    Assert.Throws<ValidationException>(() => list.AreAllStringsNotNullOrEmpty());
    }
}