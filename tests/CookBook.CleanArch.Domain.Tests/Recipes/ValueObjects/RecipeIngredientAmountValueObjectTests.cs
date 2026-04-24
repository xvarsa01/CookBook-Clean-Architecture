using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes.ValueObjects;

public class RecipeIngredientAmountValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeIngredientAmount_With_Positive_Amount_Should_Succeed()
    {
        var amount = IngredientAmount.CreateObject(1);
        Assert.True(amount.IsSuccess);
        Assert.Equal(1, amount.Value.Value);
    }
    
    [Fact]
    public void RecipeIngredientAmount_With_Same_Value_Should_Be_Equal()
    {
        var d1 = IngredientAmount.CreateObject(5);
        var d2 = IngredientAmount.CreateObject(5);

        Assert.True(d1.IsSuccess);
        Assert.True(d2.IsSuccess);
        Assert.Equal(d1.Value.Value, d2.Value.Value);
    }
    
    [Fact]
    public void Implicit_Conversion_To_Decimal_Should_Return_Underlying_Value()
    {
        var amountResult = IngredientAmount.CreateObject(2.5m);
        Assert.True(amountResult.IsSuccess);

        IngredientAmount amount = amountResult.Value;

        decimal value = amount;

        Assert.Equal(2.5m, value);
    }
    
    [Fact]
    public void Implicit_Conversion_Should_Work_In_Arithmetic_Expressions()
    {
        var amountResult = IngredientAmount.CreateObject(3m);
        Assert.True(amountResult.IsSuccess);

        IngredientAmount amount = amountResult.Value;

        decimal total = amount + 2m;

        Assert.Equal(5m, total);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeIngredientAmount_With_Zero_Amount_Should_ReturnFailure()
    {
        var amount = IngredientAmount.CreateObject(0);

        Assert.True(amount.IsFailure);
    }

    [Fact]
    public void Creating_RecipeIngredientAmount_With_Negative_Amount_Should_ReturnFailure()
    {
        var amount = IngredientAmount.CreateObject(-1);

        Assert.True(amount.IsFailure);
    }
    
    [Fact]
    public void Implicit_Conversion_With_Null_Should_Throw_NullReferenceException()
    {
        IngredientAmount? amount = null;

        Assert.Throws<NullReferenceException>(() =>
        {
            decimal _ = amount!;
        });
    }
}
