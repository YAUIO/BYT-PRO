using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class PeselValidatorTests
{
    // IsPesel();
    
    [Fact]
    public void TestPeselValidSample1()
    {
        const string pesel = "44051401458";
        pesel.IsPesel();
        Assert.True(true);
    }

    [Fact]
    public void TestPeselValidSample2()
    {
        const string pesel = "02070803628";
        pesel.IsPesel();
        Assert.True(true);
    }

    [Fact]
    public void TestPeselInvalidWrongLength()
    {
        const string pesel = "123";
        Assert.Throws<ValidationException>(() => pesel.IsPesel());
    }

    [Fact]
    public void TestPeselInvalidNonDigits()
    {
        const string pesel = "12345abc901";
        Assert.Throws<ValidationException>(() => pesel.IsPesel());
    }
}