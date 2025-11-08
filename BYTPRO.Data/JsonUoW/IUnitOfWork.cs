using BYTPRO.Data.Models;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonUoW;

public interface IUnitOfWork
{
    public JsonEntitySet<Person> Persons { get; }

    Task SaveChangesAsync();
}