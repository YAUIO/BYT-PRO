using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public class PersonRepository(IUnitOfWork uow) : IPersonRepository
{ 
    public void Add(Person person)
    {
        uow.Persons.Add(person);
    }

    public IEnumerable<Person> GetAll()
    {
        return uow.Persons.ToList();
    }
}