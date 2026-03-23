using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.Exceptions;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class IngredientAmountValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_IngredientAmount_With_Positive_Amount_Should_Succeed()
    {
        var amount = IngredientAmount.CreateObject(1);

        Assert.True(amount.IsSuccess);
        Assert.Equal(1, amount.Value.Value);
    }
    
    [Fact]
    public void IngredientAmount_With_Same_Value_Should_Be_Equal()
    {
        var a1 = IngredientAmount.CreateObject(100);
        var a2 = IngredientAmount.CreateObject(100);

        Assert.True(a1.IsSuccess);
        Assert.True(a2.IsSuccess);
        Assert.Equal(a1.Value.Value, a2.Value.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_IngredientAmount_With_Zero_Amount_Should_ReturnFailure()
    {
        var amount = IngredientAmount.CreateObject(0);

        Assert.True(amount.IsFailure);
    }

    [Fact]
    public void Creating_IngredientAmount_With_Negative_Amount_Should_ReturnFailure()
    {
        var amount = IngredientAmount.CreateObject(-1);

        Assert.True(amount.IsFailure);
    }
}
