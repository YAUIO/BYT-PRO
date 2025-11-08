using System.Reflection;

namespace BYTPRO.JsonEntityFramework.Context;

public record JsonEntityConfiguration(Type Target, string? FileName, List<PropertyInfo> One, List<PropertyInfo> Many);