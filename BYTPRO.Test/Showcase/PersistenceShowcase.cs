using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PersistenceShowcase(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";
    
    private static JsonContext GetTestContext()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);
        
        var context = new JsonContextBuilder()
            .AddJsonEntity<Person>()
                .WithFileName("person")
                .BuildEntity()
            .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .WithUoW<JsonUnitOfWork>()
            .Build();

        return context;
    }

    [Fact]
    public async Task CreatePerson()
    {
        var context = GetTestContext();
        var uow = new JsonUnitOfWork(context);
        
        var person = new Person(1, "Artiom", "Bezkorovainyi", "+48000000000", "s30000@pjwstk.edu.pl", "12345678", uow);

        await uow.SaveChangesAsync();
    }
    
    [Fact]
    public void ShowPersons()
    {
        var context = GetTestContext();
        var uow = new JsonUnitOfWork(context);
        _testOutputHelper.WriteLine(uow.Persons.ToJson());
    }
}