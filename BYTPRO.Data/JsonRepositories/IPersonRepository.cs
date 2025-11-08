using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public interface IPersonRepository
{
    public void Add(Person person);

    public IEnumerable<Person> GetAll();
}