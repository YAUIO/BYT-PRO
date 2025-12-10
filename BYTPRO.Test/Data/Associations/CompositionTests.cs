using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;

namespace BYTPRO.Test.Data.Associations;

public class CompositionTests
{
    private sealed class TestBranch : Branch
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

    private static LocalEmployee CreateLocalEmployee(Branch branch)
    {
        return new LocalEmployee(
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
    }

    [Fact]
    public void TestBranchClosureRemovesEmployees()
    {
        var branch = new TestBranch("Composition Branch");
        var employee = CreateLocalEmployee(branch);

        Assert.Contains(branch, Branch.All);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Contains(employee, branch.Employees);
        Assert.Single(branch.Employees);

        branch.CloseBranch();

        Assert.DoesNotContain(branch, Branch.All);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }

    [Fact]
    public void TestEmployeeDeletionRemovesFromBranch()
    {
        var branch = new TestBranch("Composition Branch");
        var employee = CreateLocalEmployee(branch);

        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Contains(employee, branch.Employees);
        Assert.Single(branch.Employees);

        employee.Delete();

        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }

    [Fact]
    public void TestBranchRemoveEmployeeRemovesFromBranch()
    {
        var branch = new TestBranch("Composition Branch");
        var employee = CreateLocalEmployee(branch);

        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);
        Assert.Contains(employee, branch.Employees);
        Assert.Single(branch.Employees);

        branch.RemoveEmployee(employee);

        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
        Assert.DoesNotContain(employee, branch.Employees);
    }
}