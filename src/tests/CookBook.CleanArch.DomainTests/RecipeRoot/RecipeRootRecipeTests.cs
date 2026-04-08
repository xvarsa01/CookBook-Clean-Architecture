using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.DomainTests.RecipeRoot;

public class RecipeRootRecipeTests
{
    [Fact]
    public void Creating_Recipe_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal("Cappuccino", recipe.Name.Value);
        Assert.Equal("with oak mil", recipe.Description);
        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg", recipe.ImageUrl?.Value);
        Assert.Empty(recipe.Ingredients);
    }
    
    [Fact]
    public void Creating_Recipe_WithOut_DescriptionAndImage_Should_Create()
    {
        // Act
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, null, null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal("Cappuccino", recipe.Name.Value);
        Assert.Null(recipe.Description);
        Assert.Null(recipe.ImageUrl);
        Assert.Empty(recipe.Ingredients);
    }
    
    [Fact]
    public void Creating_Recipe_With_Empty_Name_Should_ReturnFailure()
    {
        var nameResult = RecipeName.CreateObject("");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Creating_Recipe_With_Short_Name_Should_ReturnFailure()
    {
        var nameResult = RecipeName.CreateObject("AB");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Creating_Recipe_With_Negative_Duration_Should_ReturnFailure()
    {
        var durationResult = RecipeDuration.CreateObject(TimeSpan.FromMinutes(-1));

        Assert.True(durationResult.IsFailure);
    }
    
    [Fact]
    public void Creating_Recipe_With_Zero_Duration_Should_ReturnFailure()
    {
        var durationResult = RecipeDuration.CreateObject(TimeSpan.Zero);

        Assert.True(durationResult.IsFailure);
    }
    
    [Fact]
    public void Updating_RecipeName_Should_Update_Name()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateName(RecipeName.CreateObject("New").Value);

        Assert.Equal("New", recipe.Name.Value);
    }
    
    [Fact]
    public void Updating_RecipeName_ToEmpty_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        var nameResult = RecipeName.CreateObject("");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Updating_RecipeName_ToShort_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        var nameResult = RecipeName.CreateObject("AB");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Updating_RecipeName_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateName(RecipeName.CreateObject("Cappuccino").Value);

        Assert.Equal("Cappuccino", recipe.Name.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDescription_Should_Update_Description()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, null, null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Caffe).Value;

        recipe.UpdateDescription("New");

        Assert.Equal("New", recipe.Description);
    }
    
    [Fact]
    public void Updating_RecipeDescription_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "Same", null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Caffe).Value;

        recipe.UpdateDescription("Same");

        Assert.Equal("Same", recipe.Description);
    }
    
    
    [Fact]
    public void Updating_RecipeImageUrl_Should_Update_ImageUrl()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Cappuccino_in_original.jpg/1920px-Cappuccino_in_original.jpg").Value, null, null);

        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Cappuccino_in_original.jpg/1920px-Cappuccino_in_original.jpg", recipe.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, null, null);

        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg", recipe.ImageUrl?.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDuration_Should_Update_Duration()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value, null);

        Assert.Equal(TimeSpan.FromMinutes(10), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, null);

        Assert.Equal(TimeSpan.FromMinutes(5), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Negative_Value_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        var durationResult = RecipeDuration.CreateObject(TimeSpan.FromMinutes(-1));

        Assert.True(durationResult.IsFailure);
    }
    
    
    [Fact]
    public void Updating_RecipeType_Should_Update_Type()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(null, null, RecipeType.Dessert);

        Assert.Equal(RecipeType.Dessert, recipe.Type);
    }
    
    [Fact]
    public void Updating_RecipeType_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject("Cappuccino").Value, "with oak mil", ImageUrl.CreateObject("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg").Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink);

        Assert.Equal(RecipeType.Drink, recipe.Type);
    }

}
