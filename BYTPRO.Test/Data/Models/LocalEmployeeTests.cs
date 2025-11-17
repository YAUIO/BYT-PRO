using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Validation;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Data.Models;


public class LocalEmployeeTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";
    
    static LocalEmployeeTests()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);
        

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<LocalEmployee>()
            .WithFileName("localEmployees")  
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();
        
        JsonContext.SetContext(ctx);
    }
    
    [Fact]
    public void CreateLocalEmployeeWithValidData()
    {
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
            "12:00-13:00"
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
                "12:00-13:00"
            );
        });
        
        Assert.Empty(Person.All);
        Assert.Empty(Employee.All);
        Assert.Empty(LocalEmployee.All);
    }
}