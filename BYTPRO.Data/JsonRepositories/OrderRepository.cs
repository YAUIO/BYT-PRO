using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public class PersonRepository(IUnitOfWork uow) : IPersonRepository
{ 
    public async Task Add(Person person)
    {
        uow.Persons.Add(person);
        await uow.SaveChangesAsync();
    }

    public IEnumerable<Person> GetAll()
    {
        return uow.Persons.ToList();
    }
}