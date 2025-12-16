using BYTPRO.Data.Validation;
using Newtonsoft.Json;
using BYTPRO.Data.Validation.Validators;
using BYTPRO.JsonEntityFramework.Context;
using Newtonsoft.Json.Converters;

namespace BYTPRO.Data.Models.People.Employees;

public abstract class Employee : Person
{
    #region ----------< Class Extent >----------

    [JsonIgnore] private static readonly List<Employee> Extent = [];

    [JsonIgnore] public new static IReadOnlyList<Employee> All => Extent.ToList().AsReadOnly();

    #endregion

    #region ----------< Attributes >----------

    private readonly string _pesel;
    private decimal _salary;
    private EmploymentType _employmentType;

    #endregion

    #region ----------< Properties with validation >----------

    public string Pesel
    {
        get => _pesel;
        init
        {
            value.IsPesel();
            _pesel = value;
        }
    }

    public decimal Salary
    {
        get => _salary;
        set
        {
            value.IsPositive(nameof(Salary));
            _salary = value;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public EmploymentType EmploymentType
    {
        get => _employmentType;
        set
        {
            value.IsDefined(nameof(EmploymentType));
            _employmentType = value;
        }
    }

    #endregion

    #region ----------< Construction >----------

    protected Employee(
        int id,
        string name,
        string surname,
        string phone,
        string email,
        string password,
        string pesel,
        decimal salary,
        EmploymentType employmentType,
        // ----------< Roles (overlapping) >----------
        CashierParams? cashier,
        ConsultantParams? consultant,
        ManagerParams? manager)
        : base(id, name, surname, phone, email, password)
    {
        Pesel = pesel;
        Salary = salary;
        EmploymentType = employmentType;

        // ----------< Roles (overlapping) >----------
        if (cashier == null && consultant == null && manager == null)
            throw new ValidationException("Employee must have at least one role.");

        CashierRole = cashier != null ? new Cashier(cashier) : null;
        ConsultantRole = consultant != null ? new Consultant(consultant) : null;
        ManagerRole = manager != null ? new Manager(manager) : null;
    }

    protected override void OnAfterConstruction()
    {
        Extent.Add(this);
    }

    #endregion

    #region ----------< Associations >----------

    protected override void OnDelete()
    {
        Extent.Remove(this);
    }

    #endregion

    #region ----------< Inheritance >----------

    #region ----------< Roles Aspect (overlapping) >----------

    public ICashierRole? CashierRole { get; }
    public IConsultantRole? ConsultantRole { get; }
    public IManagerRole? ManagerRole { get; }

    #region ----------< Cashier (role #1) >----------

    public sealed record CashierParams(
        int RegisterCode,
        int PinCode,
        bool CanMakeReturn);

    private sealed class Cashier : ICashierRole
    {
        #region ----------< Attributes >----------

        private readonly int _registerCode;
        private readonly int _pinCode;
        private bool _canMakeReturn;

        #endregion

        #region ----------< Properties with validation >----------

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

        #endregion

        #region ----------< Constructor >----------

        internal Cashier(CashierParams cashierParams)
        {
            RegisterCode = cashierParams.RegisterCode;
            PinCode = cashierParams.PinCode;
            CanMakeReturn = cashierParams.CanMakeReturn;
        }

        #endregion
    }

    #endregion

    #region ----------< Consultant (role #2) >----------

    public sealed record ConsultantParams(
        string Specialization,
        DeserializableReadOnlyList<string> Languages);

    private sealed class Consultant : IConsultantRole
    {
        #region ----------< Attributes >----------

        private string _specialization;
        private readonly DeserializableReadOnlyList<string> _languages;

        #endregion

        #region ----------< Properties with validation >----------

        public string Specialization
        {
            get => _specialization;
            set
            {
                value.IsNotNullOrEmpty(nameof(Specialization));
                _specialization = value;
            }
        }

        public DeserializableReadOnlyList<string> Languages
        {
            get => _languages;
            init
            {
                value.AreAllStringsNotNullOrEmpty(nameof(Languages));
                _languages = value;
                _languages.MakeReadOnly();
            }
        }

        #endregion

        #region ----------< Constructor >----------

        internal Consultant(ConsultantParams consultantParams)
        {
            Specialization = consultantParams.Specialization;
            Languages = consultantParams.Languages;
        }

        #endregion
    }

    #endregion

    #region ----------< Manager (role #3) >----------

    public sealed record ManagerParams(
        ManagerialLevel ManagerialLevel);

    public enum ManagerialLevel
    {
        Junior,
        Senior
    }

    private sealed class Manager : IManagerRole
    {
        #region ----------< Attributes >----------

        private ManagerialLevel _managerialLevel;

        #endregion

        #region ----------< Properties with validation >----------

        [JsonConverter(typeof(StringEnumConverter))]
        public ManagerialLevel ManagerialLevel
        {
            get => _managerialLevel;
            set
            {
                value.IsDefined(nameof(ManagerialLevel));
                _managerialLevel = value;
            }
        }

        #endregion

        #region ----------< Constructor >----------

        internal Manager(ManagerParams managerParams)
        {
            ManagerialLevel = managerParams.ManagerialLevel;
        }

        #endregion
    }

    #endregion

    #endregion

    #endregion
}