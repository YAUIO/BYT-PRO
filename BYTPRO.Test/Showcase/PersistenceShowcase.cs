using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase(ITestOutputHelper testOutputHelper)
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";

    static PersistenceShowcase()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);

        var context = new JsonContextBuilder()
            // ----------< People >----------
            .AddJsonEntity<Customer>()
            .WithFileName("customers")
            .BuildEntity()
            // ------------------------------
            .AddJsonEntity<LocalEmployee>()
            .WithFileName("localEmployees")
            .BuildEntity()
            //------------------------------
            .AddJsonEntity<RegionalEmployee>()
            .WithFileName("regionalEmployees")
            .BuildEntity()

            //------------------------------
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();

        JsonContext.SetContext(context);
    }


    [Fact]
    public void TestPeopleClassExtent()
    {
        var customer = new Customer(
            1,
            "Artiom",
            "Bezkorovainyi",
            "+48000000000",
            "s30000@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        var localEmployee = new LocalEmployee(
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

        var regionalEmployee = new RegionalEmployee(
            3,
            "Jane",
            "Smith",
            "+48123456780",
            "jane.smith@gmail.com",
            "123456789",
            "12345678902",
            10000m,
            EmploymentType.Intern,
            "INTERN@12345",
            SupervisionScope.Technical
        );
        
        JsonContext.Context.SaveChanges();

        ShowAll();
    }

    [Fact]
    public void ShowAll()
    {
        testOutputHelper.WriteLine($"Persons({Person.All.Count}): {Person.All.ToJson()}");
        testOutputHelper.WriteLine("\n\n----------------------------------------\n\n");
        testOutputHelper.WriteLine($"Customers({Customer.All.Count}): {Customer.All.ToJson()}");
        testOutputHelper.WriteLine("\n\n----------------------------------------\n\n");
        testOutputHelper.WriteLine($"Employees({Employee.All.Count}): {Employee.All.ToJson()}");
        testOutputHelper.WriteLine("\n\n----------------------------------------\n\n");
        testOutputHelper.WriteLine($"LocalEmployees({LocalEmployee.All.Count}): {LocalEmployee.All.ToJson()}");
        testOutputHelper.WriteLine("\n\n----------------------------------------\n\n");
        testOutputHelper.WriteLine($"RegionalEmployees({RegionalEmployee.All.Count}): {RegionalEmployee.All.ToJson()}");
    }

    [Fact]
    public void TestReadOnly()
    {
        var emp = JsonContext.Context.GetTable<LocalEmployee>().First();
        Assert.Throws<NotSupportedException>(() => emp.TrainingsCompleted.Add("something-should-throw"));
        Assert.True(emp.TrainingsCompleted.IsReadOnly);
    }

    [Fact]
    public void Delete()
    {
        if (Directory.Exists(DbRoot)) Directory.Delete(DbRoot, true);
    }
}