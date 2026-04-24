using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes.ValueObjects;

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
    
    // implicit operators:
    [Fact]
    public void Implicit_Conversion_To_TimeSpan_Should_Return_Underlying_Value()
    {
        var duration = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        Assert.True(duration.IsSuccess);
    
        TimeSpan value = duration.Value;
    
        Assert.Equal(TimeSpan.FromMinutes(5), value);
    }
    
    [Fact]
    public void GreaterOrEqual_Operator_Should_Work_For_Greater_And_Equal_Values()
    {
        var longer = RecipeDuration.CreateObject(TimeSpan.FromMinutes(10));
        var shorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var equalToShorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
    
        Assert.True(longer.IsSuccess);
        Assert.True(shorter.IsSuccess);
        Assert.True(equalToShorter.IsSuccess);
    
        Assert.True(longer.Value >= shorter.Value);
        Assert.True(shorter.Value >= equalToShorter.Value);
        Assert.False(shorter.Value >= longer.Value);
    }
    
    [Fact]
    public void LessOrEqual_Operator_Should_Work_For_Less_And_Equal_Values()
    {
        var longer = RecipeDuration.CreateObject(TimeSpan.FromMinutes(10));
        var shorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var equalToShorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
    
        Assert.True(longer.IsSuccess);
        Assert.True(shorter.IsSuccess);
        Assert.True(equalToShorter.IsSuccess);
    
        Assert.True(shorter.Value <= longer.Value);
        Assert.True(shorter.Value <= equalToShorter.Value);
        Assert.False(longer.Value <= shorter.Value);
    }
    
    [Fact]
    public void GreaterThan_Operator_Should_Work_For_Strict_Comparison()
    {
        var longer = RecipeDuration.CreateObject(TimeSpan.FromMinutes(10));
        var shorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var equalToShorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
    
        Assert.True(longer.IsSuccess);
        Assert.True(shorter.IsSuccess);
        Assert.True(equalToShorter.IsSuccess);
    
        Assert.True(longer.Value > shorter.Value);
        Assert.False(shorter.Value > longer.Value);
        Assert.False(shorter.Value > equalToShorter.Value);
    }
    
    [Fact]
    public void LessThan_Operator_Should_Work_For_Strict_Comparison()
    {
        var longer = RecipeDuration.CreateObject(TimeSpan.FromMinutes(10));
        var shorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
        var equalToShorter = RecipeDuration.CreateObject(TimeSpan.FromMinutes(5));
    
        Assert.True(longer.IsSuccess);
        Assert.True(shorter.IsSuccess);
        Assert.True(equalToShorter.IsSuccess);
    
        Assert.True(shorter.Value < longer.Value);
        Assert.False(longer.Value < shorter.Value);
        Assert.False(shorter.Value < equalToShorter.Value);
    }
    
}
