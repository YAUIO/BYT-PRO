using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public interface IPersonRepository
{
    public Task Add(Person person);

    public IEnumerable<Person> GetAll();
}