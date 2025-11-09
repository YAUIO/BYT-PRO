using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Jef.Context;

public class JsonContextTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestDb";

    private static int _contexts;
    
    public static JsonContext GetTestContext(int sets = 0, string? root = null)
    {
        if (Directory.Exists(DbRoot) && _contexts == 0 && sets == 0) Directory.Delete(DbRoot, true);
        else if (!Directory.Exists(DbRoot)) Directory.CreateDirectory(DbRoot);
        
        _contexts++;
        
        return new JsonContextBuilder()
            .AddJsonEntity<TestModel>()
                .WithFileName("test")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(root ?? $"{DbRoot}/{_contexts}_{sets}"))
            .Build();
    }

    [Fact]
    public void TestFileCreation()
    {
        var context = GetTestContext();

        Assert.True(File.Exists(context.GetTable<TestModel>().Path));
    }
    
    [Fact]
    public void TestFileLoading()
    {
        var root = $"{DbRoot}/TestLoading/";
        
        var context = GetTestContext(root:root);
        
        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        Assert.True(File.Exists(context.GetTable<TestModel>().Path));
        
        context.GetTable<TestModel>()
            .Add(model);
        context.SaveChanges();
        
        var newContext = new JsonContextBuilder()
            .AddJsonEntity<TestModel>()
                .WithFileName("test")
                .BuildEntity()
            .WithRoot(new DirectoryInfo(root))
            .Build();

        Assert.Contains(model, newContext.GetTable<TestModel>());
        
        Assert.Equal(context.GetTable<TestModel>(), newContext.GetTable<TestModel>());
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnAdd()
    {
        var context = GetTestContext();

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));

        table.Add(model);

        Assert.Equal("[]", await File.ReadAllTextAsync(table.Path));

        await context.SaveChangesAsync();

        Assert.NotEmpty(await File.ReadAllTextAsync(table.Path));

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnDelete()
    {
        var context = GetTestContext();

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

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

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        table.Add(model);

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));

        table.Clear();

        await context.SaveChangesAsync();

        Assert.Equal(table.ToJson(), await File.ReadAllTextAsync(table.Path));
    }
    
    [Fact]
    public void TestSaveChangesUpdatesOnAdd()
    {
        var context = GetTestContext();

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));

        table.Add(model);

        Assert.Equal("[]", File.ReadAllText(table.Path));

        context.SaveChanges();

        Assert.NotEmpty(File.ReadAllText(table.Path));

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));
    }

    [Fact]
    public void TestSaveChangesUpdatesOnDelete()
    {
        var context = GetTestContext();

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

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

        var table = context.GetTable<TestModel>();

        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        table.Add(model);

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));

        table.Clear();

        context.SaveChanges();

        Assert.Equal(table.ToJson(), File.ReadAllText(table.Path));
    }
}