using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public interface IUnitOfWork
{
    public JsonEntitySet<Person> Persons { get; }
    public JsonEntitySet<Order> Orders { get; }

    Task SaveChangesAsync();
}