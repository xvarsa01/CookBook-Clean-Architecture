using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Ingredients;

public class IngredientTests
{
    private const string ValidName = "milk";
    private const string ValidDescription = "oak milk";
    private const string ValidImageUrl = "https://i.imgur.com/YYPzexp.png";
    private const string NewValidImageUrl = "https://i.imgur.com/v76AHPz.jpeg";

    [Fact]
    public void Creating_Ingredient_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, ingredient.Id);
        Assert.Equal(ValidName, ingredient.Name);
        Assert.Equal(ValidDescription, ingredient.Description);
        Assert.Equal(ValidImageUrl, ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Creating_Ingredient_WithOut_DescriptionAndImage_Should_Create()
    {
        // Act
        var ingredient = Ingredient.Create(ValidName, null, null).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, ingredient.Id);
        Assert.Equal(ValidName, ingredient.Name);
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
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateName("New");

        Assert.Equal("New", ingredient.Name);
    }
    
    [Fact]
    public void Updating_IngredientName_ToEmpty_Should_Throw()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        var updateResult = ingredient.UpdateName(string.Empty);

        Assert.True(updateResult.IsFailure);
    }
    
    [Fact]
    public void Updating_Ingredient_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateName(ValidName);

        Assert.Equal(ValidName, ingredient.Name);
    }
    
    
    [Fact]
    public void Updating_IngredientDescription_Should_Update_Description()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateDescription("New");

        Assert.Equal("New", ingredient.Description);
    }
    
    [Fact]
    public void Updating_IngredientDescription_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateDescription(ValidDescription);

        Assert.Equal(ValidDescription, ingredient.Description);
    }
    
    
    [Fact]
    public void Updating_IngredientImageUrl_Should_Update_ImageUrl()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateImageUrl(ImageUrl.CreateObject(NewValidImageUrl).Value);

        Assert.Equal(NewValidImageUrl, ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        ingredient.UpdateImageUrl(ImageUrl.CreateObject(ValidImageUrl).Value);

        Assert.Equal(ValidImageUrl, ingredient.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Invalid_Value_Should_ReturnFailure()
    {
        var ingredient = Ingredient.Create(ValidName, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value).Value;

        var imageUrlResult = ImageUrl.CreateObject("a.png ");

        Assert.True(imageUrlResult.IsFailure);
    }
    
}
