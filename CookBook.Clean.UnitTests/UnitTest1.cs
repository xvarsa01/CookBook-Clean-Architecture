using CookBook.Clean.Application;
using Xunit;

namespace CookBook.Clean.UnitTests;

public class UseCaseResultTests
{
    [Fact]
    public void Ok_ReturnsSuccessWithValue()
    {
        var result = UseCaseResult<string>.Ok("value");
        Assert.True(result.Success);
        Assert.Equal("value", result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void NotFound_ReturnsFailureWithError()
    {
        var result = UseCaseResult<int>.NotFound("not found");
        Assert.False(result.Success);
        Assert.Equal(default(int), result.Value);
        Assert.Equal("not found", result.Error);
    }
}