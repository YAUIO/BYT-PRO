using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Factories;

internal static class PeopleFactory
{
    #region ----------< Default values >----------

    // Person
    private static int _personIdCounter = 1;
    private static int DefaultId() => Interlocked.Increment(ref _personIdCounter);
    private static string DefaultName() => "John";
    private static string DefaultSurname() => "Doe";
    private static string DefaultPhone() => "+1000000000";
    private static string DefaultEmail(int id) => $"john{id}.doe@edu.pl";
    private static string DefaultPassword() => "test123";

    // Customer
    private static DateTime DefaultRegistrationDate() => new(2025, 12, 10);

    // Employee
    private static string DefaultPesel() => "12345678901";
    private static decimal DefaultSalary() => 5000m;
    private static EmploymentType DefaultEmploymentType() => EmploymentType.FullTime;

    // Roles
    private static Employee.CashierParams DefaultCashierParams() =>
        new(321, 123456, true);

    private static Employee.ConsultantParams DefaultConsultantParams() =>
        new("Technician", ["English", "Polish"]);

    private static Employee.ManagerParams DefaultManagerParams() =>
        new(Employee.ManagerialLevel.Junior);

    // LocalEmployee
    private static DeserializableReadOnlyList<string> DefaultTrainings() => ["Safety", "Onboarding"];
    private static string DefaultBreakSchedule() => "12:00-12:45";

    // RegionalEmployee
    private static string DefaultBadgeNumber() => "BN-123";
    private static SupervisionScope DefaultSupervisionScope() => SupervisionScope.Technical;

    #endregion

    #region ----------< Customer Factory methods >----------

    public static Customer CreateCustomer(
        // ----------< Person >----------
        int? id = null,
        string? name = null,
        string? surname = null,
        string? phone = null,
        string? email = null,
        string? password = null,
        // ----------< Customer >----------
        DateTime? registrationDate = null)
    {
        var realId = id ?? DefaultId();
        return new Customer(
            // ----------< Person >----------
            realId,
            name ?? DefaultName(),
            surname ?? DefaultSurname(),
            phone ?? DefaultPhone(),
            email ?? DefaultEmail(realId),
            password ?? DefaultPassword(),
            // ----------< Customer >----------
            registrationDate ?? DefaultRegistrationDate()
        );
    }

    #endregion

    #region ----------< Employee Factory methods >----------

    public static LocalEmployee CreateLocalEmployee(
        // ----------< LocalEmployee (required) >----------
        Branch branch,
        // ----------< Person >----------
        int? id = null,
        string? name = null,
        string? surname = null,
        string? phone = null,
        string? email = null,
        string? password = null,
        // ----------< Employee >----------
        string? pesel = null,
        decimal? salary = null,
        EmploymentType? employmentType = null,
        Employee.CashierParams? cashier = null,
        Employee.ConsultantParams? consultant = null,
        Employee.ManagerParams? manager = null,
        // ----------< LocalEmployee >----------
        DeserializableReadOnlyList<string>? trainingsCompleted = null,
        string? breakSchedule = null,
        // ----------< Flags >----------
        bool useDefaultRolesWhenNoneProvided = true)
    {
        var realId = id ?? DefaultId();
        var noRolesProvided = cashier == null && consultant == null && manager == null;
        var assignDefaultRoles = noRolesProvided && useDefaultRolesWhenNoneProvided;
        return new LocalEmployee(
            // ----------< Person >----------
            realId,
            name ?? DefaultName(),
            surname ?? DefaultSurname(),
            phone ?? DefaultPhone(),
            email ?? DefaultEmail(realId),
            password ?? DefaultPassword(),
            // ----------< Employee >----------
            pesel ?? DefaultPesel(),
            salary ?? DefaultSalary(),
            employmentType ?? DefaultEmploymentType(),
            assignDefaultRoles ? DefaultCashierParams() : cashier,
            assignDefaultRoles ? DefaultConsultantParams() : consultant,
            assignDefaultRoles ? DefaultManagerParams() : manager,
            // ----------< LocalEmployee >----------
            trainingsCompleted ?? DefaultTrainings(),
            breakSchedule ?? DefaultBreakSchedule(),
            branch
        );
    }

    public static RegionalEmployee CreateRegionalEmployee(
        // ----------< Person >----------
        int? id = null,
        string? name = null,
        string? surname = null,
        string? phone = null,
        string? email = null,
        string? password = null,
        // ----------< Employee >----------
        string? pesel = null,
        decimal? salary = null,
        EmploymentType? employmentType = null,
        Employee.CashierParams? cashier = null,
        Employee.ConsultantParams? consultant = null,
        Employee.ManagerParams? manager = null,
        // ----------< RegionalEmployee >----------
        string? badgeNumber = null,
        SupervisionScope? supervisionScope = null,
        // ----------< Flags >----------
        bool useDefaultRolesWhenNoneProvided = true)
    {
        var realId = id ?? DefaultId();
        var noRolesProvided = cashier == null && consultant == null && manager == null;
        var assignDefaultRoles = noRolesProvided && useDefaultRolesWhenNoneProvided;
        return new RegionalEmployee(
            // ----------< Person >----------
            realId,
            name ?? DefaultName(),
            surname ?? DefaultSurname(),
            phone ?? DefaultPhone(),
            email ?? DefaultEmail(realId),
            password ?? DefaultPassword(),
            // ----------< Employee >----------
            pesel ?? DefaultPesel(),
            salary ?? DefaultSalary(),
            employmentType ?? DefaultEmploymentType(),
            assignDefaultRoles ? DefaultCashierParams() : cashier,
            assignDefaultRoles ? DefaultConsultantParams() : consultant,
            assignDefaultRoles ? DefaultManagerParams() : manager,
            // ----------< RegionalEmployee >----------
            badgeNumber ?? DefaultBadgeNumber(),
            supervisionScope ?? DefaultSupervisionScope()
        );
    }

    #endregion
}