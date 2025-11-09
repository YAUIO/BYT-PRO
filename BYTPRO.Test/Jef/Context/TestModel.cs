namespace BYTPRO.Test.Jef.Context;

public class TestModel
{
    public int Id { get; init; }
    public string? Value { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj.GetType() != GetType()) return false;

        var casted = obj as TestModel;

        return casted.Id == Id && casted.Value == Value;
    }

    public override int GetHashCode() => HashCode.Combine(Id);
}