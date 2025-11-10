using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Employees.Roles;

public class Cashier
{
    // ----------< Attributes >----------
    private readonly int _registerCode;
    private readonly int _pinCode;
    private bool _canMakeReturn;


    // ----------< Properties with validation >----------
    public int RegisterCode
    {
        get => _registerCode;
        init
        {
            value.IsNonNegative(nameof(RegisterCode));
            _registerCode = value;
        }
    }

    public int PinCode
    {
        get => _pinCode;
        init
        {
            value.IsPositive(nameof(PinCode));
            _pinCode = value;
        }
    }

    public bool CanMakeReturn
    {
        get => _canMakeReturn;
        set
        {
            // Do not convert into auto-property.
            // Maybe some business rule will be added here
            _canMakeReturn = value;
        }
    }


    // ----------< Constructor >----------
    public Cashier(
        int registerCode,
        int pinCode,
        bool canMakeReturn)
    {
        RegisterCode = registerCode;
        PinCode = pinCode;
        CanMakeReturn = canMakeReturn;
    }
}