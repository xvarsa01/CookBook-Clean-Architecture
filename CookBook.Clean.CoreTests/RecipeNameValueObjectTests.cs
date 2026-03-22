using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class RecipeNameValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeName_With_AtLeat_3_Characters_Should_Succeed()
    {
        var name = new RecipeName("aaa");
        Assert.Equal("aaa", name.Value);
    }
    
    [Fact]
    public void IngredientAmount_With_Same_Value_Should_Be_Equal()
    {
        var n1 = new RecipeName("longer name");
        var n2 = new RecipeName("longer name");

        Assert.Equal(n1.Value, n2.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeName_With_LessThen_3_Characters_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                new RecipeName("aa")
        );
    }

    [Fact]
    public void Creating_RecipeName_With_Empty_String_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new RecipeName(string.Empty)
        );
    }
}
