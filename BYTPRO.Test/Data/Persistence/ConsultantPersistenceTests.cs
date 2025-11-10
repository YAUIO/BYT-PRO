using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Employees;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Models.Orders;
using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BYTPRO.Test.Data.Persistence;

public class ConsultantPersistenceTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestConsultantDb";

    private static int _contexts;

    private static JsonContext GetTestContext(int sets = 0, string? root = null)
    {
        if (Directory.Exists(DbRoot) && _contexts == 0 && sets == 0) Directory.Delete(DbRoot, true);
        else if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);
        
        _contexts++;
        
        var context = new JsonContextBuilder()
            .AddJsonEntity<Person>()
                .WithFileName("person")
                .BuildEntity()
            .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(root ?? $"{DbRoot}/{_contexts}_{sets}"))
            .WithUoW<JsonUnitOfWork>()
            .Build();

        return context;
    }

    private static Consultant GetTestConsultant(JsonContext context)
    {
        return new Consultant(1, "Artiom", "Bezkorovainyi", "+48000000000", "s300000@pjwstk.edu.pl", "12345678", "111111111", (decimal)10.1, EmploymentType.FullTime, "Consulting", new JsonUnitOfWork(context));
    }

    [Fact]
    public void TestFileCreation()
    {
        var context = GetTestContext();

        Assert.True(File.Exists(context.GetTable<Person>().Path));
    }
    
    [Fact]
    public void TestFileLoading()
    {
        var root = $"{DbRoot}/TestConsultantLoading/";

        var context = GetTestContext(root: root);
        
        try
        {
            var model = GetTestConsultant(context);

            context.GetTable<Person>()
                .Add(model);
            context.SaveChanges();

            var newContext = new JsonContextBuilder()
                .AddJsonEntity<Person>()
                .WithFileName("person")
                .BuildEntity()
                .AddJsonEntity<Order>()
                .WithFileName("order")
                .BuildEntity()
                .WithRoot(new DirectoryInfo(root))
                .WithUoW<JsonUnitOfWork>()
                .Build();

            Assert.Contains(model, newContext.GetTable<Person>());

            Assert.Equal(context.GetTable<Person>(), newContext.GetTable<Person>());
        }
        catch (IOException)
        {
            throw new Exception($"{JsonSerializer.Serialize(context.FileLocks)}");
        }
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnDelete()
    {
        var context = GetTestContext();

        var table = context.GetTable<Person>();

        var model = GetTestConsultant(context);

        table.Add(model);

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));

        table.Remove(model);

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));
    }
    
    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnClear()
    {
        var context = GetTestContext();

        var table = context.GetTable<Person>();

        var model = GetTestConsultant(context);

        table.Add(model);

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));

        table.Clear();

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));
    }

    [Fact]
    public void TestSaveChangesUpdatesOnDelete()
    {
        var context = GetTestContext();

        var table = context.GetTable<Person>();

        var model = GetTestConsultant(context);

        table.Add(model);

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));

        table.Remove(model);

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));
    }
    
    [Fact]
    public void TestSaveChangesUpdatesOnClear()
    {
        var context = GetTestContext();

        var table = context.GetTable<Person>();

        var model = GetTestConsultant(context);

        table.Add(model);

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));

        table.Clear();

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));
    }
}