using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Jef.Context;

public class JsonEntitySetTests
{
    private static int _sets;
    
    private static JsonEntitySet<TestModel> GetTestSet()
    {
        _sets++;
        return JsonContextTests.GetTestContext(_sets).GetTable<TestModel>();
    }

    [Fact]
    public void TestGetTypeReturnsTestModel()
    {
        Assert.Equal(typeof(TestModel), GetTestSet().GetGenericType());
    }

    [Fact]
    public void TestAddAddsToSet()
    {
        var set = GetTestSet();
        
        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };
        
        Assert.DoesNotContain(model, set);
        
        set.Add(model);

        Assert.Contains(model, set);
    }
    
    [Fact]
    public void TestClearClearsSet()
    {
        var set = GetTestSet();
        
        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };
        
        set.Add(model);
        
        Assert.Contains(model, set);

        set.Clear();
        
        Assert.Empty(set);
    }
    
    [Fact]
    public void TestRemoveRemovesFromSet()
    {
        var set = GetTestSet();
        
        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };
        
        set.Add(model);
        
        Assert.Contains(model, set);

        set.Remove(model);
        
        Assert.DoesNotContain(model, set);
    }
}