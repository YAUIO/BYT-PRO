using System.Text.Json;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Jef.Context;

public class JsonSerializeTests
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

        Assert.Contains(JsonSerializer.Serialize(model), list.ToJson());
    }
    
    [Fact]
    public void TestToJsonReturnsNotEmptyForEmpty()
    {
        Assert.Equal("{}", new List<TestModel>().ToJson());
    }
}