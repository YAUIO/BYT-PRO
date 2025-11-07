using BYTPRO.Data.Models;
using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Data.JsonRepositories;

public class PersonRepository(JsonContext context)
{
    private readonly DynamicSet<Person> _persons = context.GetTable<Person>();

    public void Add(Person person)
    {
        _persons.Add(person);
    }

    public List<Person> GetAll()
    {
        return _persons.ToList();
    }
}