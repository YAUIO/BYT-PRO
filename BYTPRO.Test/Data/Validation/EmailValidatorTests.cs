using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class EmailValidatorTests
{
    // IsEmail();
    
    [Fact]
    public void TestEmailValid()
    {
        const string email = "user.name+tag@sub.domain.com";
        email.IsEmail();
        Assert.True(true);
    }

    [Fact]
    public void TestEmailInvalidNoAt()
    {
        const string email = "user.domain.com";
        Assert.Throws<ValidationException>(() => email.IsEmail());
    }

    [Fact]
    public void TestEmailInvalidBadDomain()
    {
        const string email = "a@b.";
        Assert.Throws<ValidationException>(() => email.IsEmail());
    }
}