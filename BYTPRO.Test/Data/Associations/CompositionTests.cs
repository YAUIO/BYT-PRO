using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Local;
using BYTPRO.Test.Data.Factories;

namespace BYTPRO.Test.Data.Associations;

public class CompositionTests
{
    [Fact]
    public void TestRemovingWholeRemovesPartsAndWhole()
    {
        // Create whole
        var branch = LocationsFactory.CreatePureBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create parts
        var employee = PeopleFactory.CreateLocalEmployee(branch);
        Assert.Equal(employee.Branch, branch);
        Assert.Contains(employee, branch.Employees);
        Assert.Contains(employee, LocalEmployee.All);
        Assert.Contains(employee, Employee.All);
        Assert.Contains(employee, Person.All);

        var employee2 = PeopleFactory.CreateLocalEmployee(branch);
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
        var branch = LocationsFactory.CreatePureBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create part
        var employee = PeopleFactory.CreateLocalEmployee(branch);
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

        // Whole still exists
        Assert.Contains(branch, Branch.All);
    }

    [Fact]
    public void TestRemovingPartFromWholeSide()
    {
        // Create whole
        var branch = LocationsFactory.CreatePureBranch();
        Assert.Contains(branch, Branch.All);
        Assert.Empty(branch.Employees);

        // Create part
        var employee = PeopleFactory.CreateLocalEmployee(branch);
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

        // Whole still exists
        Assert.Contains(branch, Branch.All);
    }
}