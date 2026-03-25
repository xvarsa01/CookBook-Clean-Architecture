using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests.IngredientRoot;

public class IngredientRootTests
{
    [Fact]
    public void Creating_Ingredient_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, ingredient.Id);
        Assert.Equal("milk", ingredient.Name);
        Assert.Equal("oak milk", ingredient.Description);
        Assert.Equal("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg", ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Creating_Ingredient_WithOut_DescriptionAndImage_Should_Create()
    {
        // Act
        var ingredient = Ingredient.Create("milk", null, null).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, ingredient.Id);
        Assert.Equal("milk", ingredient.Name);
        Assert.Null(ingredient.Description);
        Assert.Null(ingredient.ImageUrl);
    }
    
    [Fact]
    public void Creating_Ingredient_With_Empty_Name_Should_ReturnFailure()
    {
        var result = Ingredient.Create(string.Empty, null, null);

        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void Creating_Ingredient_With_Invalid_ImageUrl_Should_ReturnFailure()
    {
        var imageUrlResult = ImageUrl.CreateObject("a.png ");

        Assert.True(imageUrlResult.IsFailure);
    }
    
    
    [Fact]
    public void Updating_IngredientName_Should_Update_Name()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateName("New");

        Assert.Equal("New", ingredient.Name);
    }
    
    [Fact]
    public void Updating_IngredientName_ToEmpty_Should_Throw()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        var updateResult = ingredient.UpdateName(string.Empty);

        Assert.True(updateResult.IsFailure);
    }
    
    [Fact]
    public void Updating_Ingredient_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateName("milk");

        Assert.Equal("milk", ingredient.Name);
    }
    
    
    [Fact]
    public void Updating_IngredientDescription_Should_Update_Description()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateDescription("New");

        Assert.Equal("New", ingredient.Description);
    }
    
    [Fact]
    public void Updating_IngredientDescription_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateDescription("oak milk");

        Assert.Equal("oak milk", ingredient.Description);
    }
    
    
    [Fact]
    public void Updating_IngredientImageUrl_Should_Update_ImageUrl()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateImageUrl(ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Steamed_milk.jpg").Value);

        Assert.Equal("https://en.wikipedia.org/wiki/Milk#/media/File:Steamed_milk.jpg", ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        ingredient.UpdateImageUrl(ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value);

        Assert.Equal("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg", ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Invalid_Value_Should_ReturnFailure()
    {
        var ingredient = Ingredient.Create("milk", "oak milk", ImageUrl.CreateObject("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg").Value).Value;

        var imageUrlResult = ImageUrl.CreateObject("a.png ");

        Assert.True(imageUrlResult.IsFailure);
    }
    
}
