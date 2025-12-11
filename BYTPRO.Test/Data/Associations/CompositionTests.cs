using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;

namespace BYTPRO.Test.Data.Associations;

public class CompositionTests
{
    private static Store CreateBranch()
    {
        return new Store(
            new Address("Street", "1", null, "00-000", "City"),
            "Composition Branch",
            "09:00-17:00",
            250m,
            5,
            150m,
            2
        );
    }

    private static LocalEmployee CreateLocalEmployee(Branch branch)
    {
        return new LocalEmployee(
            Math.Abs(Guid.NewGuid().GetHashCode()),
            "John",
            "Doe",
            "+48111222333",
            $"john.doe{Math.Abs(Guid.NewGuid().GetHashCode())}@example.com",
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
    public void TestRemovingWholeRemovesPartsAndWhole()
    {
        // Create whole
        var branch = CreateBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create parts
        var employee = CreateLocalEmployee(branch);
        Assert.Equal(employee.Branch, branch);
        Assert.Contains(employee, branch.Employees);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);

        var employee2 = CreateLocalEmployee(branch);
        Assert.Equal(employee2.Branch, branch);
        Assert.Contains(employee2, branch.Employees);
        Assert.Contains(employee2, LocalEmployee.All);
        Assert.Contains(employee2, Employee.All);
        Assert.Contains(employee2, Person.All);

        // Removing the "whole" => must remove all its "parts" (LocalEmployees) and the "whole" (Branch) itself.
        branch.CloseBranch();

        // Parts removed
        Assert.DoesNotContain(employee, branch.Employees);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);

        Assert.DoesNotContain(employee2, branch.Employees);
        Assert.DoesNotContain(employee2, LocalEmployee.All);
        Assert.DoesNotContain(employee2, Employee.All);
        Assert.DoesNotContain(employee2, Person.All);

        Assert.Empty(branch.Employees);

        // Whole removed
        Assert.DoesNotContain(branch, Branch.All);
    }

    [Fact]
    public void TestRemovingPartFromPartSide()
    {
        // Create whole
        var branch = CreateBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create part
        var employee = CreateLocalEmployee(branch);
        Assert.Equal(employee.Branch, branch);
        Assert.Contains(employee, branch.Employees);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);

        // Removing the "part" => must remove "part" (LocalEmployee) from the "whole" (Branch).
        employee.Delete();

        // Part removed
        Assert.DoesNotContain(employee, branch.Employees);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
    }

    [Fact]
    public void TestRemovingPartFromWholeSide()
    {
        // Create whole
        var branch = CreateBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create part
        var employee = CreateLocalEmployee(branch);
        Assert.Equal(employee.Branch, branch);
        Assert.Contains(employee, branch.Employees);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);

        // Removing the "part" => must remove "part" (LocalEmployee) from the "whole" (Branch).
        branch.RemoveEmployee(employee);

        // Part removed
        Assert.DoesNotContain(employee, branch.Employees);
        Assert.DoesNotContain(employee, LocalEmployee.All);
        Assert.DoesNotContain(employee, Employee.All);
        Assert.DoesNotContain(employee, Person.All);
    }
}