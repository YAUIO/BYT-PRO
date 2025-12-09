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

    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/BYT_PRO_TESTS/Composition.json";

    private static void ResetContext(bool removeContext = true)
    {
        if (File.Exists(DbRoot) && removeContext)
            File.Delete(DbRoot);

        new JsonContextBuilder()
            .AddJsonEntity<LocalEmployee>()
            .AddJsonEntity<TestBranch>()
            .BuildWithDbRoot(DbRoot);
    }

    [Fact]
    private void TestBranchClosureRemovesEmployees()
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

        branch.CloseBranch();

        Assert.DoesNotContain(branch, Branch.All);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }

    // Test Reverse connections

    [Fact]
    private void TestEmployeeDeletionRemovesFromBranch()
    {
        ResetContext();

        var branch = new TestBranch("Composition Branch");

        var employee = new LocalEmployee(
            100,
            "Bob",
            "Johnson",
            "+48111222555",
            "bob.johnson@example.com",
            "password789",
            "90010112347",
            5500m,
            EmploymentType.FullTime,
            ["Safety"],
            "12:00",
            branch
        );

        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Single(branch.Employees);

        employee.Delete();

        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }

    [Fact]
    private void TestBranchRemoveEmployeeRemovesFromBranch()
    {
        ResetContext();

        var branch = new TestBranch("Composition Branch");

        var employee = new LocalEmployee(
            100,
            "Bob",
            "Johnson",
            "+48111222555",
            "bob.johnson@example.com",
            "password789",
            "90010112347",
            5500m,
            EmploymentType.FullTime,
            ["Safety"],
            "12:00",
            branch
        );

        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Single(branch.Employees);

        branch.RemoveEmployee(employee);

        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }
}