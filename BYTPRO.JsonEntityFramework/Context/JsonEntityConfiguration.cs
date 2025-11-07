namespace BYTPRO.JsonEntityFramework.Context;

public record JsonEntityConfiguration(Type Target, string? FileName, List<JsonConnection> One, List<JsonConnection> Many);