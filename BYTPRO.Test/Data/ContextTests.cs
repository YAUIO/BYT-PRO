using BYTPRO.Data.JsonRepositories;
using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.JsonEntityFramework.Context;
using Xunit.Abstractions;

namespace BYTPRO.Test.Data;

public class ContextTests
{
    [Fact]
    public async Task Test()
    {
        var context = new JsonContextBuilder()
            .AddJsonEntity<Person>()
            .WithFileName("person")
            .BuildEntity()
            .WithRoot(new DirectoryInfo("F:\\DbTest"))
            .Build();

        var uow = new JsonUnitOfWork(context);

        var repo = new PersonRepository(uow);
        
        repo.Add(new Person()
        {
            Id = 1,
            Name = "name",
            Surname = "surname",
            Email = "ar@ar",
            Password = "asd",
            Phone = "123",
        });
        
        repo.Add(new Person()
        {
            Id = 2,
            Name = "name",
            Surname = "surname",
            Email = "ar@ar",
            Password = "asd",
            Phone = "123",
        });
        
        Assert.Equal(2, repo.GetAll().ToList().Count);

        await context.SaveChangesAsync();

        using var file = new StreamReader(File.OpenRead(@"F:\DbTest\person.json"));
        
        var content = await file.ReadToEndAsync();
        
        Assert.Equal(uow.Persons.GetJson(), content);
        
        file.Close();
    }
}