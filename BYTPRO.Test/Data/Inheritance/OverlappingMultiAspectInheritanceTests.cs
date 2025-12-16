using System.ComponentModel.DataAnnotations;
using BYTPRO.Data.Models.Locations;
using BYTPRO.Data.Models.Locations.Branches;
using BYTPRO.Data.Models.People.Employees;
using BYTPRO.Data.Models.People.Employees.Regional;
using BYTPRO.Test.Data.Factories;
using ValidationException = BYTPRO.Data.Validation.ValidationException;

namespace BYTPRO.Test.Data.Inheritance;

public class OverlappingMultiAspectInheritanceTests
{
    
    // ----------< RegionalEmployee Inheritance Tests >----------
    
    [Fact]
    public void CreateRegionalEmployeeWithOneRole()
    {
        var employee = PeopleFactory.CreateRegionalEmployee(
            cashier: new Employee.CashierParams(999, 123, false)
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.Null(employee.ConsultantRole);
        Assert.Null(employee.ManagerRole);

        // Role values
        Assert.Equal(999, employee.CashierRole.RegisterCode);
        Assert.Equal(123, employee.CashierRole.PinCode);
        Assert.False(employee.CashierRole.CanMakeReturn);
    }

    [Fact]
    public void CreateRegionalEmployeeWithTwoRoles()
    {
        var employee = PeopleFactory.CreateRegionalEmployee(
            cashier: new Employee.CashierParams(888, 456, true),
            consultant: new Employee.ConsultantParams("Tech", ["English"])
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.NotNull(employee.ConsultantRole);
        Assert.Null(employee.ManagerRole);

        // Role values
        Assert.Equal(888, employee.CashierRole.RegisterCode);
        Assert.Equal(456, employee.CashierRole.PinCode);
        Assert.True(employee.CashierRole.CanMakeReturn);

        Assert.Equal("Tech", employee.ConsultantRole.Specialization);
        Assert.Contains("English", employee.ConsultantRole.Languages);
    }

    [Fact]
    public void CreateRegionalEmployeeWithThreeRoles()
    {
        var employee = PeopleFactory.CreateRegionalEmployee(
            cashier: new Employee.CashierParams(777, 789, false),
            consultant: new Employee.ConsultantParams("PC Tech", ["Polish"]),
            manager: new Employee.ManagerParams(Employee.ManagerialLevel.Senior)
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.NotNull(employee.ConsultantRole);
        Assert.NotNull(employee.ManagerRole);

        // Role values
        Assert.Equal(777, employee.CashierRole.RegisterCode);
        Assert.Equal(789, employee.CashierRole.PinCode);
        Assert.False(employee.CashierRole.CanMakeReturn);

        Assert.Equal("PC Tech", employee.ConsultantRole.Specialization);
        Assert.Contains("Polish", employee.ConsultantRole.Languages);

        Assert.Equal(Employee.ManagerialLevel.Senior, employee.ManagerRole.ManagerialLevel);
    }
    
    [Fact]
    public void CreateRegionalEmployeeWithNoRolesFails()
    {
        Assert.Throws<ValidationException>(() => PeopleFactory.CreateRegionalEmployeeWithNoRolesDefaults());
    }
    
    [Fact]
    public void RoleApiDoesntProvideConstructors()
    {
        var emp = PeopleFactory.CreateRegionalEmployee();
        
        Assert.Empty(emp.CashierRole.GetType().GetConstructors());
        Assert.Empty(emp.ManagerRole.GetType().GetConstructors());
        Assert.Empty(emp.ConsultantRole.GetType().GetConstructors());
    }
    
    // ----------< LocalEmployee Inheritance Tests >----------
    private static class BranchTester
    {
        public sealed class TestBranch : Branch
        {
            public TestBranch(string name) : base(
                new Address("Street", "1", null, "00-000", "City"),
                name, "09-18", 100m)
            {
                FinishConstruction();
            }
        }
    }
    [Fact]
    public void CreateLocalEmployeeWithOneRole()
    {
        var branch = new BranchTester.TestBranch("LocalTestBranch"); 
        
        var employee = PeopleFactory.CreateLocalEmployee(
            branch: branch,
            cashier: new Employee.CashierParams(999, 123, false)
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.Null(employee.ConsultantRole);
        Assert.Null(employee.ManagerRole);

        // Role values
        Assert.Equal(999, employee.CashierRole.RegisterCode);
        Assert.Equal(123, employee.CashierRole.PinCode);
        Assert.False(employee.CashierRole.CanMakeReturn);
    }

    [Fact]
    public void CreateLocalEmployeeWithTwoRoles()
    {
        var branch = new BranchTester.TestBranch("LocalTestBranch"); 
        
        var employee = PeopleFactory.CreateLocalEmployee(
            branch: branch,
            cashier: new Employee.CashierParams(888, 456, true),
            consultant: new Employee.ConsultantParams("Tech", ["English"])
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.NotNull(employee.ConsultantRole);
        Assert.Null(employee.ManagerRole);

        // Role values
        Assert.Equal(888, employee.CashierRole.RegisterCode);
        Assert.Equal(456, employee.CashierRole.PinCode);
        Assert.True(employee.CashierRole.CanMakeReturn);

        Assert.Equal("Tech", employee.ConsultantRole.Specialization);
        Assert.Contains("English", employee.ConsultantRole.Languages);
    }

    [Fact]
    public void CreateLocalEmployeeWithThreeRoles()
    {
        var branch = new BranchTester.TestBranch("LocalTestBranch"); 
        
        var employee = PeopleFactory.CreateLocalEmployee(
            branch: branch,
            cashier: new Employee.CashierParams(777, 789, false),
            consultant: new Employee.ConsultantParams("PC Tech", ["Polish"]),
            manager: new Employee.ManagerParams(Employee.ManagerialLevel.Senior)
        );

        // Roles presence
        Assert.NotNull(employee.CashierRole);
        Assert.NotNull(employee.ConsultantRole);
        Assert.NotNull(employee.ManagerRole);

        // Role values
        Assert.Equal(777, employee.CashierRole.RegisterCode);
        Assert.Equal(789, employee.CashierRole.PinCode);
        Assert.False(employee.CashierRole.CanMakeReturn);

        Assert.Equal("PC Tech", employee.ConsultantRole.Specialization);
        Assert.Contains("Polish", employee.ConsultantRole.Languages);

        Assert.Equal(Employee.ManagerialLevel.Senior, employee.ManagerRole.ManagerialLevel);
    }

    [Fact]
    public void CreateLocalEmployeeWithNoRolesFails()
    {
        var branch = new BranchTester.TestBranch("LocalTestBranch"); 
        
        Assert.Throws<ValidationException>(() => PeopleFactory.CreateLocalEmployeeWithNoRolesDefaults(branch));
    }

    [Fact]
    public void LocalRoleApiDoesntProvideConstructors()
    {
        var branch = new BranchTester.TestBranch("LocalTestBranch"); 
        var emp = PeopleFactory.CreateLocalEmployee(branch: branch);
        
        Assert.Empty(emp.CashierRole.GetType().GetConstructors());
        Assert.Empty(emp.ManagerRole.GetType().GetConstructors());
        Assert.Empty(emp.ConsultantRole.GetType().GetConstructors());
    }
    
}