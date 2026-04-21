using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.DomainTests.Shared;

public class StronglyTypedIdsTests
{
    private sealed record TestStringId(string? Value) : StronglyTypedId<string?>(Value);

    private sealed record TestGuidId(Guid Value) : StronglyTypedId(Value);

    [Fact]
    public void Generic_ToString_With_NonNull_Value_Should_Return_Value_String()
    {
        TestStringId id = new TestStringId("abc-123");

        var actual = id.ToString();

        Assert.Equal("abc-123", actual);
    }

    [Fact]
    public void Generic_ToString_With_Null_Value_Should_Return_Empty_String()
    {
        var id = new TestStringId(null);

        var actual = id.ToString();

        Assert.Equal(string.Empty, actual);
    }

    [Fact]
    public void Generic_Implicit_String_Operator_Should_Return_ToString_Result()
    {
        var id = new TestStringId("my-id");

        string actual = id;

        Assert.Equal("my-id", actual);
    }

    [Fact]
    public void Guid_Based_ToString_Should_Return_Guid_String()
    {
        var guid = Guid.NewGuid();
        var id = new TestGuidId(guid);

        var actual = id.ToString();

        Assert.Equal(guid.ToString(), actual);
    }

    [Fact]
    public void Guid_Implicit_Operator_Should_Return_Underlying_Guid()
    {
        var guid = Guid.NewGuid();
        var id = new TestGuidId(guid);

        Guid actual = id;

        Assert.Equal(guid, actual);
    }

    [Fact]
    public void StronglyTypedId_Should_Implement_Marker_Interface()
    {
        var id = new TestGuidId(Guid.NewGuid());

        Assert.IsAssignableFrom<IStronglyTypedId>(id);
    }
}
