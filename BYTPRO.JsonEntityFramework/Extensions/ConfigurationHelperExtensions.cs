using System.Reflection;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class ConfigurationHelperExtensions
{
    public static PropertyInfo GetVirtualProperty(this Type type, string name)
    {
        return type
            .GetProperties()
            .Single(p => p.Name.Equals(name));
    } 
}