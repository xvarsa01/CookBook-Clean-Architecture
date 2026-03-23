using CookBook.Clean.Application;
using CookBook.Clean.Core;
using Xunit;

namespace CookBook.Clean.UnitTests;

public class ResultTests
{
    [Fact]
    public void Ok_ReturnsSuccessWithValue()
    {
        var result = Result<string>.Ok("value");
        Assert.True(result.Success);
        Assert.Equal("value", result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void NotFound_ReturnsFailureWithError()
    {
        var result = Result<int>.NotFound("not found");
        Assert.False(result.Success);
        Assert.Equal(default(int), result.Value);
        Assert.Equal("not found", result.Error);
    }
}