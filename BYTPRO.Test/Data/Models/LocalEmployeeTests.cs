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
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TDb";

    static LocalEmployeeTests()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<LocalEmployee>()
            .WithFileName("localEmployees")
            .BuildEntity()
            .AddJsonEntity<Store>() 
            .WithFileName("stores")
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();

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
            2,
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

        Assert.Single(Person.All);
        Assert.Single(Employee.All);
        Assert.Single(LocalEmployee.All);
        Assert.Same(local, Person.All.Single());
        Assert.Same(local, Employee.All.Single());
        Assert.Same(local, LocalEmployee.All.Single());
    }

    [Fact]
    public void CreateLocalEmployeeWithInvalidData()
    {
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

        Assert.Empty(Person.All);
        Assert.Empty(Employee.All);
        Assert.Empty(LocalEmployee.All);
    }
}