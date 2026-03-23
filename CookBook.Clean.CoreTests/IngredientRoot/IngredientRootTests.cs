using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.Exceptions;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests.IngredientRoot;

public class IngredientRootTests
{
    [Fact]
    public void Creating_Ingredient_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));
        
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
        var ingredient = new IngredientEntity("milk",null, null);
        
        // Assert
        Assert.NotEqual(Guid.Empty, ingredient.Id);
        Assert.Equal("milk", ingredient.Name);
        Assert.Null(ingredient.Description);
        Assert.Null(ingredient.ImageUrl);
    }
    
    [Fact]
    public void Creating_Ingredient_With_Empty_Name_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                new IngredientEntity(string.Empty, null, null)
        );
    }
    
    [Fact]
    public void Creating_Ingredient_With_Invalid_ImageUrl_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new IngredientEntity("milk", null, new ImageUrl("a.png "))
        );
    }
    
    
    [Fact]
    public void Updating_IngredientName_Should_Update_Name()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateName("New");

        Assert.Equal("New", ingredient.Name);
    }
    
    [Fact]
    public void Updating_IngredientName_ToEmpty_Should_Throw()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        Assert.Throws<ArgumentException>(() =>
            ingredient.UpdateName(string.Empty)
        );
    }
    
    [Fact]
    public void Updating_Ingredient_To_Same_Value_Should_Not_Change()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateName("milk");

        Assert.Equal("milk", ingredient.Name);
    }
    
    
    [Fact]
    public void Updating_IngredientDescription_Should_Update_Description()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateDescription("New");

        Assert.Equal("New", ingredient.Description);
    }
    
    [Fact]
    public void Updating_IngredientDescription_To_Same_Value_Should_Not_Change()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateDescription("oak milk");

        Assert.Equal("oak milk", ingredient.Description);
    }
    
    
    [Fact]
    public void Updating_IngredientImageUrl_Should_Update_ImageUrl()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateImageUrl(new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Steamed_milk.jpg"));

        Assert.Equal("https://en.wikipedia.org/wiki/Milk#/media/File:Steamed_milk.jpg", ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        ingredient.UpdateImageUrl(new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        Assert.Equal("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg", ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Invalid_Value_Should_Throw()
    {
        var ingredient = new IngredientEntity("milk", "oak milk", new ImageUrl("https://en.wikipedia.org/wiki/Milk#/media/File:Glass_of_Milk_(33657535532).jpg"));

        Assert.Throws<InvalidImageUrlException>(() =>
            ingredient.UpdateImageUrl(new ImageUrl("a.png "))
        );
    }
    
}
