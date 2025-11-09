using System.Reflection;
using BYTPRO.JsonEntityFramework.Extensions;

namespace BYTPRO.Test.Jef.Context;

public class ConfigurationHelperExtensionsTests
{
    [Fact]
    public void TestGetVirtualPropertyReturnsProperty()
    {
        var type = typeof(TestModel);

        const string propName = "Id";
        
        var prop = type.GetPropertyByName(propName);

        Assert.IsAssignableFrom<PropertyInfo>(prop);
        Assert.Equal(propName, prop.Name);
    }
}