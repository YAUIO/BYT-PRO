using BYTPRO.JsonEntityFramework.Context;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Jef.Context;

public class JsonContextTests
{
    private static string DbRoot => $"{Directory.GetCurrentDirectory()}/TestDb";

    private static int _contexts;

    public static JsonContext GetTestContext(int sets = 0, string? root = null)
    {
        if (Directory.Exists(DbRoot) && _contexts == 0 && sets == 0)
        {
            Directory.Delete(DbRoot, true);
        }
        else if (!Directory.Exists(DbRoot))
        {
            Directory.CreateDirectory(DbRoot);
        }

        _contexts++;

        var context = new JsonContextBuilder()
            .AddJsonEntity<TestModel>()
            .WithDbFile(new FileInfo(root ?? $"{DbRoot}/{_contexts}_{sets}"))
            .Build();

        return context;
    }

    [Fact]
    public void TestFileCreation()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        Assert.True(File.Exists(context.DbPath));
    }

    [Fact]
    public void TestFileLoading()
    {
        var root = $"{DbRoot}/TestLoading/";

        var context = GetTestContext(root: root);

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        Assert.True(File.Exists(context.DbPath));

        context.GetTable<TestModel>()
            .Add(model);
        context.SaveChanges();

        var newContext = new JsonContextBuilder()
            .AddJsonEntity<TestModel>()
            .WithDbFile(new FileInfo(root))
            .Build();

        Assert.Contains(model, newContext.GetTable<TestModel>());

        Assert.Equal(context.GetTable<TestModel>(), newContext.GetTable<TestModel>());
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnAdd()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        await context.SaveChangesAsync();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        var text = await File.ReadAllTextAsync(context.DbPath);

        table.Add(model);

        await context.SaveChangesAsync();

        Assert.NotEmpty(await File.ReadAllTextAsync(context.DbPath));

        Assert.NotEqual(text, await File.ReadAllTextAsync(context.DbPath));
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnDelete()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        await context.SaveChangesAsync();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        table.Add(model);

        await context.SaveChangesAsync();

        var text = await File.ReadAllTextAsync(context.DbPath);

        table.Remove(model);

        await context.SaveChangesAsync();

        Assert.NotEqual(text, await File.ReadAllTextAsync(context.DbPath));
    }

    [Fact]
    public async Task TestSaveChangesAsyncUpdatesOnClear()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        await context.SaveChangesAsync();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        table.Add(model);

        await context.SaveChangesAsync();

        var text = await File.ReadAllTextAsync(context.DbPath);

        table.Clear();

        await context.SaveChangesAsync();

        Assert.NotEqual(text, await File.ReadAllTextAsync(context.DbPath));
    }

    [Fact]
    public void TestSaveChangesUpdatesOnAdd()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        var text = File.ReadAllText(context.DbPath);

        table.Add(model);

        context.SaveChanges();

        Assert.NotEmpty( File.ReadAllText(context.DbPath));

        Assert.NotEqual(text, File.ReadAllText(context.DbPath));
    }

    [Fact]
    public void TestSaveChangesAUpdatesOnDelete()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        table.Add(model);

        context.SaveChanges();

        var text = File.ReadAllText(context.DbPath);

        table.Remove(model);

        context.SaveChanges();

        Assert.NotEqual(text, File.ReadAllText(context.DbPath));
    }

    [Fact]
    public void TestSaveChangesUpdatesOnClear()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        table.Add(model);

        context.SaveChanges();

        var text = File.ReadAllText(context.DbPath);

        table.Clear();

        context.SaveChanges();

        Assert.NotEqual(text, File.ReadAllText(context.DbPath));
    }

    [Fact]
    public async Task TestRollbackAsync()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        await context.SaveChangesAsync();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        var stagedText = await File.ReadAllTextAsync(context.DbPath);

        table.Add(model);

        Assert.Equal(stagedText, await File.ReadAllTextAsync(context.DbPath));

        Assert.Contains(model, table);

        await context.RollbackAsync();

        Assert.Equal(stagedText, await File.ReadAllTextAsync(context.DbPath));

        Assert.Empty(table);
    }

    [Fact]
    public void TestRollback()
    {
        var context = GetTestContext();

        context.GetTable<TestModel>().Clear();
        context.SaveChanges();

        var table = context.GetTable<TestModel>();

        var model = new TestModel
        {
            Id = 1,
            Value = "value"
        };

        var stagedText = File.ReadAllText(context.DbPath);

        table.Add(model);

        Assert.Equal(stagedText, File.ReadAllText(context.DbPath));

        Assert.Contains(model, table);

        context.Rollback();

        Assert.Equal(stagedText, File.ReadAllText(context.DbPath));

        Assert.Empty(table);
    }
}