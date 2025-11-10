using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase(ITestOutputHelper testOutputHelper)
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";
    
    static PersistenceShowcase()
    {
        if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);
        
        var context = new JsonContextBuilder()
            .AddJsonEntity<Customer>()
                .WithFileName("customer")
                .BuildEntity()
            .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();

        JsonContext.SetContext(context);
    }
    
    [Fact]
    public async Task CreatePerson()
    {
        var context = JsonContext.Context;
        
        var person = new Customer(
            1,
            "Artiom", 
            "Bezkorovainyi",
            "+48000000000", 
            "s30000@pjwstk.edu.pl", 
            "12345678",
            DateTime.Now
        );

        await new JsonUnitOfWork(context).SaveChangesAsync();
        
        testOutputHelper.WriteLine($"After create {Person.All.ToJson()}");
    }
        
    
    [Fact]
    public async Task TestPersistence()
    {
        var context = JsonContext.Context;
        var uow = new JsonUnitOfWork(context);
        
        var person = new Customer(
            1,
            "Artiom", 
            "Bezkorovainyi",
            "+48000000000", 
            "s30000@pjwstk.edu.pl", 
            "12345678",
            DateTime.Now
        );

        await uow.SaveChangesAsync();
        
        testOutputHelper.WriteLine($"After add {Person.All.ToJson()}");
        
        person.Remove();
        
        await uow.SaveChangesAsync();
        
        testOutputHelper.WriteLine($"After delete {Person.All.ToJson()}");
    }
    
    [Fact]
    public void ShowPersons()
    {
        var context = JsonContext.Context;
        testOutputHelper.WriteLine(context.GetTable<Customer>().ToJson());
    }
    
    [Fact]
    public void RemoveDb()
    {
        Directory.Delete(DbRoot, true);
    }
}