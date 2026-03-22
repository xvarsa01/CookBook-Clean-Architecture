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
        var amount = new IngredientAmount(1);
        Assert.Equal(1, amount.Value);
    }
    
    [Fact]
    public void IngredientAmount_With_Same_Value_Should_Be_Equal()
    {
        var a1 = new IngredientAmount(100);
        var a2 = new IngredientAmount(100);

        Assert.Equal(a1.Value, a2.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_IngredientAmount_With_Zero_Amount_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                new IngredientAmount(0)
        );
    }

    [Fact]
    public void Creating_IngredientAmount_With_Negative_Amount_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new IngredientAmount(-1)
        );
    }
}
