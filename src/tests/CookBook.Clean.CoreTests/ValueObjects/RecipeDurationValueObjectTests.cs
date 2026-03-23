using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.Exceptions;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class RecipeDurationValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeDuration_With_Positive_Duration_Should_Succeed()
    {
        var duration = RecipeDuration.CreateObject(TimeSpan.FromSeconds(1));
        Assert.Equal(TimeSpan.FromSeconds(1), duration.Value);
    }
    
    [Fact]
    public void RecipeDuration_With_Same_Value_Should_Be_Equal()
    {
        var d1 = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var d2 = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));

        Assert.Equal(d1.Value, d2.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeDuration_With_Zero_Duration_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                RecipeDuration.CreateObject(TimeSpan.Zero)
        );
    }

    [Fact]
    public void Creating_RecipeDuration_With_Negative_Duration_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            RecipeDuration.CreateObject(TimeSpan.FromSeconds(-1))
        );
    }
}
