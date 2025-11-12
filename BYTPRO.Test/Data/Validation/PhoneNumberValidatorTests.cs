using BYTPRO.Data.Validation.Validators;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Validation;

public class PhoneNumberValidatorTests
{
    // IsPhoneNumber();
    
    [Fact]
    public void TestPhoneValid()
    {
        const string phone = "+48123456789";
        phone.IsPhoneNumber();
        Assert.True(true);
    }

    [Fact]
    public void TestPhoneInvalidTooShort()
    {
        const string phone = "12345";
        Assert.Throws<ValidationException>(() => phone.IsPhoneNumber());
    }

    [Fact]
    public void TestPhoneInvalidWithLetters()
    {
        const string phone = "+48 12A 456 789";
        Assert.Throws<ValidationException>(() => phone.IsPhoneNumber());
    }
}