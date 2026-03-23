using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class RecipeNameValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeName_With_AtLeat_3_Characters_Should_Succeed()
    {
        var name = RecipeName.CreateObject("aaa");
        Assert.Equal("aaa", name.Value);
    }
    
    [Fact]
    public void IngredientAmount_With_Same_Value_Should_Be_Equal()
    {
        var n1 = RecipeName.CreateObject("longer name");
        var n2 = RecipeName.CreateObject("longer name");

        Assert.Equal(n1.Value, n2.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeName_With_LessThen_3_Characters_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                RecipeName.CreateObject("aa")
        );
    }

    [Fact]
    public void Creating_RecipeName_With_Empty_String_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            RecipeName.CreateObject(string.Empty)
        );
    }
}
