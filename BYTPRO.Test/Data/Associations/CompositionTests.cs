using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.JsonEntityFramework.Context;


namespace BYTPRO.Test.Data.Associations;

public class BranchCompositionTest
{
    public class TestBranch : Branch
    {
        public TestBranch(string name) : base(
            new Address("Street", "1", null, "00-000", "City"),
            name,
            "09:00-17:00",
            100m)
        {
            FinishConstruction();
        }
    }

    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestCompositionDb";

    private static void ResetContext(bool removeContext = true)
    {
        if (File.Exists(DbRoot) && removeContext)
            File.Delete(DbRoot);

        var ctx = new JsonContextBuilder()
            .AddJsonEntity<LocalEmployee>()
            .AddJsonEntity<TestBranch>()
            .BuildWithDbFile(new FileInfo(DbRoot));

        JsonContext.SetContext(ctx);
    }

    [Fact]
    private void TestBranchDeletionCascadesToEmployees()
    {
        ResetContext();

        var branch = new TestBranch("Composition Branch");

        var employee = new LocalEmployee(
            100,
            "John",
            "Doe",
            "+48111222333",
            "john.doe@example.com",
            "password123",
            "90010112345",
            5000m,
            EmploymentType.FullTime,
            ["Safety"],
            "12:00",
            branch
        );

        Assert.Contains(branch, Branch.All);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Single(branch.Employees);

        branch.Delete();

        Assert.DoesNotContain(branch, Branch.All);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.Empty(branch.Employees);
    }
}