using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes.ValueObjects;

public class RecipeNameValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_RecipeName_With_AtLeat_3_Characters_Should_Succeed()
    {
        var name = RecipeName.CreateObject("aaa");
        Assert.True(name.IsSuccess);
        Assert.Equal("aaa", name.Value.Value);
    }
    
    [Fact]
    public void RecipeName_With_Same_Value_Should_Be_Equal()
    {
        var n1 = RecipeName.CreateObject("longer name");
        var n2 = RecipeName.CreateObject("longer name");

        Assert.True(n1.IsSuccess);
        Assert.True(n2.IsSuccess);
        Assert.Equal(n1.Value.Value, n2.Value.Value);
    }
    
    //invalid ones:
    [Fact]
    public void Creating_RecipeName_With_LessThen_3_Characters_Should_ReturnFailure()
    {
        var name = RecipeName.CreateObject("aa");

        Assert.True(name.IsFailure);
    }

    [Fact]
    public void Creating_RecipeName_With_Empty_String_Should_ReturnFailure()
    {
        var name = RecipeName.CreateObject(string.Empty);

        Assert.True(name.IsFailure);
    }
}
