using BYTPRO.Data.Models.People;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Xunit.Abstractions;

namespace BYTPRO.Test.Showcase;

public class PersistenceShowcase(ITestOutputHelper testOutputHelper)
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/Db";

    static PersistenceShowcase()
    {
        if (Directory.Exists(DbRoot)) Directory.Delete(DbRoot, true);
        Directory.CreateDirectory(DbRoot);

        var context = new JsonContextBuilder()
            .AddJsonEntity<Customer>()
            .WithFileName("customers")
            .BuildEntity()
            .WithRoot(new DirectoryInfo(DbRoot))
            .Build();

        JsonContext.SetContext(context);
    }


    [Fact]
    public async Task TestPersistence()
    {
        var customer = new Customer(
            1,
            "Artiom",
            "Bezkorovainyi",
            "+48000000000",
            "s30000@pjwstk.edu.pl",
            "12345678",
            DateTime.Now
        );

        await JsonContext.Context.SaveChangesAsync();

        testOutputHelper.WriteLine($"Persons: {Person.All.ToJson()}");
        testOutputHelper.WriteLine($"\nCustomers: {Customer.All.ToJson()}");
    }
}