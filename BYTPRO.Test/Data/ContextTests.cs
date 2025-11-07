using BYTPRO.Data.JsonRepositories;
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

        var repo = new PersonRepository(context);
        
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
        
        Assert.Equal(2, repo.GetAll().Count);

        await context.SaveChangesAsync();
    }
}