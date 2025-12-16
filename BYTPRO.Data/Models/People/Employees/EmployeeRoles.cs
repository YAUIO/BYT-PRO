using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.Models.People.Employees;

public interface ICashierRole
{
    int RegisterCode { get; }
    int PinCode { get; }
    bool CanMakeReturn { get; }
}

public interface IConsultantRole
{
    string Specialization { get; }
    DeserializableReadOnlyList<string> Languages { get; }
}

public interface IManagerRole
{
    Employee.ManagerialLevel ManagerialLevel { get; }
}