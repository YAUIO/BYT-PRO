using System.Text.Json;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Jef.Context;

public class JsonSerializerExtensionsTests
{
    [Fact]
    public void TestToJsonContainsObject()
    {
        var model = new TestModel()
        {
            Id = 1,
            Value = "value",
        };

        List<TestModel> list = [model];

        Assert.Contains($"Id", list.ToJson());
        Assert.Contains($"{model.Id}", list.ToJson());
        
        Assert.Contains($"Value", list.ToJson());
        Assert.Contains($"{model.Value}", list.ToJson());
    }
    
    [Fact]
    public void TestToJsonReturnsNotEmptyForEmpty()
    {
        Assert.Equal("[]", new List<TestModel>().ToJson());
    }
}