using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Data.Models;

public class LocalEmployeeTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/BYT_PRO_TESTS/LocalEmployee.json";

    static LocalEmployeeTests()
    {
        if (File.Exists(DbRoot))
            File.Delete(DbRoot);

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<LocalEmployee>()
            .AddJsonEntity<Store>()
            .BuildWithDbRoot(DbRoot);

        JsonContext.SetContext(ctx);
    }

    private static Store CreateTestStore()
    {
        var address = new Address("Main St", "Warsaw", null, "00-001", "Poland");

        return new Store(
            address,
            "Test Store",
            "08:00-22:00",
            100m,
            5,
            80m,
            2
        );
    }

    [Fact]
    public void CreateLocalEmployeeWithValidData()
    {
        var store = CreateTestStore();

        var local = new LocalEmployee(
            3,
            "John",
            "Smith",
            "+48123456789",
            "john.smith@gmail.com",
            "12345",
            "12345678901",
            5000m,
            EmploymentType.FullTime,
            ["Basics"],
            "12:00-13:00",
            store
        );

        Assert.Contains(local, Person.All);
        Assert.Contains(local, Employee.All);
        Assert.Contains(local, LocalEmployee.All);
    }

    [Fact]
    public void CreateLocalEmployeeWithInvalidData()
    {
        var persons = Person.All;
        var emps = Employee.All;
        var localemps = LocalEmployee.All;

        Assert.Throws<ValidationException>(() =>
        {
            var store = CreateTestStore();

            var local = new LocalEmployee(
                2,
                "John",
                "Smith",
                "+48123456789",
                "john",
                "12345",
                "12345678901",
                5000m,
                EmploymentType.FullTime,
                ["Onboarding", ""],
                "12:00-13:00",
                store
            );
        });

        Assert.Equal(persons, Person.All);
        Assert.Equal(emps, Employee.All);
        Assert.Equal(localemps, LocalEmployee.All);
    }
}