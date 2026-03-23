using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.CoreTests.ValueObjects;

public class RecipeDurationValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeDuration_With_Positive_Duration_Should_Succeed()
    {
        var duration = RecipeDuration.CreateObject(TimeSpan.FromSeconds(1));
        Assert.True(duration.IsSuccess);
        Assert.Equal(TimeSpan.FromSeconds(1), duration.Value.Value);
    }
    
    [Fact]
    public void RecipeDuration_With_Same_Value_Should_Be_Equal()
    {
        var d1 = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var d2 = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));

        Assert.True(d1.IsSuccess);
        Assert.True(d2.IsSuccess);
        Assert.Equal(d1.Value.Value, d2.Value.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeDuration_With_Zero_Duration_Should_ReturnFailure()
    {
        var duration = RecipeDuration.CreateObject(TimeSpan.Zero);

        Assert.True(duration.IsFailure);
    }

    [Fact]
    public void Creating_RecipeDuration_With_Negative_Duration_Should_ReturnFailure()
    {
        var duration = RecipeDuration.CreateObject(TimeSpan.FromSeconds(-1));

        Assert.True(duration.IsFailure);
    }
}
