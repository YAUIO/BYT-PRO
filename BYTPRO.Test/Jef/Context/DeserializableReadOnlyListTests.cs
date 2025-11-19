using BYTPRO.JsonEntityFramework.Context;

namespace BYTPRO.Test.Jef.Context;

public class DeserializableReadOnlyListTests
{
    [Fact]
    public void TestStores()
    {
        var list = new DeserializableReadOnlyList<string>();

        var str = "hello";

        list.Add(str);

        Assert.Contains(str, list);
    }

    [Fact]
    public void TestStoresAfterReadOnly()
    {
        var list = new DeserializableReadOnlyList<string>();

        var str = "hello";

        list.Add(str);

        list.MakeReadOnly();

        Assert.Contains(str, list);
    }

    [Fact]
    public void TestReadOnly()
    {
        var list = new DeserializableReadOnlyList<string>();

        var str = "hello";

        list.Add(str);

        list.MakeReadOnly();

        Assert.Contains(str, list);
        Assert.Throws<NotSupportedException>(() => list.Add(str));
        Assert.True(list.IsReadOnly);
    }
}