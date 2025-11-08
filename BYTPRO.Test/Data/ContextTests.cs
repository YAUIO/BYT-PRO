using BYTPRO.Data.JsonRepositories;
using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
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
            .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
            .WithRoot(new DirectoryInfo("F:\\DbTest"))
            .Build();

        var uow = new JsonUnitOfWork(context);

        var repo = new PersonRepository(uow);
        
        var orderRepo = new OrderRepository(uow);
        
        await repo.Add(new Person()
        {
            Id = 1,
            Name = "name",
            Surname = "surname",
            Email = "ar@ar",
            Password = "asd",
            Phone = "123",
        });
        
        await repo.Add(new Person()
        {
            Id = 2,
            Name = "name",
            Surname = "surname",
            Email = "ar@ar",
            Password = "asd",
            Phone = "123",
        });
        
        await orderRepo.Add(new Order()
        {
            Id = 1,
            Status = "new",
            CreationDate = DateTime.Today,
            Customer = repo.GetAll().ToList()[0],
        });
        
        Assert.Equal(2, repo.GetAll().ToList().Count);
        
        Assert.Single(orderRepo.GetAll().ToList());

        using var file = new StreamReader(File.OpenRead(@"F:\DbTest\person.json"));
        
        var content = await file.ReadToEndAsync();
        
        Assert.Equal(uow.Persons.GetJson(), content);
        
        file.Close();
    }
}