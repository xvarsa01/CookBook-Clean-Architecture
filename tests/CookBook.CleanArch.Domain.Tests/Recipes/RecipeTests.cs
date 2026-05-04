using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes;

public class RecipeTests
{
    private const string ValidName = "Cappuccino";
    private const string ValidDescription = "with oak mil";
    private const string ValidImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg";
    private const string NewValidImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Cappuccino_in_original.jpg/1920px-Cappuccino_in_original.jpg";
    
    private static IReadOnlyCollection<RecipeCreateIngredient> DefaultIngredients() =>
    [
        new(new IngredientId(Guid.NewGuid()),
            IngredientAmount.CreateObject(100).Value,
            MeasurementUnit.Ml)
    ];

    [Fact]
    public void Creating_Recipe_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal(ValidName, recipe.Name.Value);
        Assert.Equal(ValidDescription, recipe.Description);
        Assert.Equal(ValidImageUrl, recipe.ImageUrl?.Value);
        Assert.Single(recipe.Ingredients);
    }
    
    [Fact]
    public void Creating_Recipe_WithOut_DescriptionAndImage_Should_Create()
    {
        // Act
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, null, null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal(ValidName, recipe.Name.Value);
        Assert.Null(recipe.Description);
        Assert.Null(recipe.ImageUrl);
        Assert.Single(recipe.Ingredients);
    }

    [Fact]
    public void Creating_Recipe_Without_Ingredients_Should_ReturnFailure()
    {
        var result = Recipe.Create(
            RecipeName.CreateObject(ValidName).Value,
            ValidDescription,
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value,
            RecipeType.Drink,
            []);

        Assert.True(result.IsFailure);
        Assert.Contains("must contain at least 1 ingredient", result.Error.Message);
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
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateName(RecipeName.CreateObject("New").Value);

        Assert.Equal("New", recipe.Name.Value);
    }
    
    [Fact]
    public void Updating_RecipeName_ToEmpty_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        var nameResult = RecipeName.CreateObject("");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Updating_RecipeName_ToShort_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        var nameResult = RecipeName.CreateObject("AB");

        Assert.True(nameResult.IsFailure);
    }
    
    [Fact]
    public void Updating_RecipeName_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateName(RecipeName.CreateObject(ValidName).Value);

        Assert.Equal(ValidName, recipe.Name.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDescription_Should_Update_Description()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, null, null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Caffe, DefaultIngredients()).Value;

        recipe.UpdateDescription("New");

        Assert.Equal("New", recipe.Description);
    }
    
    [Fact]
    public void Updating_RecipeDescription_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, "Same", null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Caffe, DefaultIngredients()).Value;

        recipe.UpdateDescription("Same");

        Assert.Equal("Same", recipe.Description);
    }
    
    
    [Fact]
    public void Updating_RecipeImageUrl_Should_Update_ImageUrl()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(ImageUrl.CreateObject(NewValidImageUrl).Value, null, null);

        Assert.Equal(NewValidImageUrl, recipe.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(ImageUrl.CreateObject(ValidImageUrl).Value, null, null);

        Assert.Equal(ValidImageUrl, recipe.ImageUrl?.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDuration_Should_Update_Duration()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value, null);

        Assert.Equal(TimeSpan.FromMinutes(10), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, null);

        Assert.Equal(TimeSpan.FromMinutes(5), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Negative_Value_Should_ReturnFailure()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        var durationResult = RecipeDuration.CreateObject(TimeSpan.FromMinutes(-1));
        if (durationResult.IsSuccess)
        {
            recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(-1)).Value, null);
        }

        Assert.True(durationResult.IsFailure);
    }
    
    [Fact]
    public void UpdateRest_WhenAllParametersAreNull_ShouldNotChangeTheRecipe()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var originalName = recipe.Name;
        var originalDescription = recipe.Description;
        var originalImageUrl = recipe.ImageUrl;
        var originalDuration = recipe.Duration;
        var originalType = recipe.Type;
        var originalIngredients = recipe.Ingredients.ToList();

        // Act
        recipe.UpdateRest(null, null, null);

        // Assert
        Assert.Equal(originalName, recipe.Name);
        Assert.Equal(originalDescription, recipe.Description);
        Assert.Equal(originalImageUrl, recipe.ImageUrl);
        Assert.Equal(originalDuration, recipe.Duration);
        Assert.Equal(originalType, recipe.Type);
        Assert.Equal(originalIngredients, recipe.Ingredients);
    }
    
    [Fact]
    public void Updating_RecipeType_Should_Update_Type()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(null, null, RecipeType.Dessert);

        Assert.Equal(RecipeType.Dessert, recipe.Type);
    }
    
    [Fact]
    public void Updating_RecipeType_To_Same_Value_Should_Not_Change()
    {
        var recipe = Recipe.Create(RecipeName.CreateObject(ValidName).Value, ValidDescription, ImageUrl.CreateObject(ValidImageUrl).Value, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink, DefaultIngredients()).Value;

        recipe.UpdateRest(null, RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value, RecipeType.Drink);

        Assert.Equal(RecipeType.Drink, recipe.Type);
    }
    
    [Fact]
    public void Creating_Recipe_With_Initial_Ingredients_Should_Create_With_Those_Ingredients()
    {
        var ingredients = new List<RecipeCreateIngredient>
        {
            new(
                new IngredientId(Guid.NewGuid()),
                IngredientAmount.CreateObject(2).Value,
                MeasurementUnit.Pieces),
            new(
                new IngredientId(Guid.NewGuid()),
                IngredientAmount.CreateObject(300).Value,
                MeasurementUnit.Ml)
        };

        var result = Recipe.Create(
            RecipeName.CreateObject("Mojito").Value,
            "fresh drink",
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value,
            RecipeType.Drink,
            ingredients);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Ingredients.Count);
    }

    [Fact]
    public void Creating_Recipe_With_More_Than_10_Ingredients_Should_ReturnFailure()
    {
        var ingredients = Enumerable.Range(0, 11)
            .Select(_ => new RecipeCreateIngredient(
                new IngredientId(Guid.NewGuid()),
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces))
            .ToList();

        var result = Recipe.Create(
            RecipeName.CreateObject("Mojito").Value,
            "fresh drink",
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value,
            RecipeType.Drink,
            ingredients);

        Assert.True(result.IsFailure);
        Assert.Contains("can not have more than 10 ingredients", result.Error.Message);
    }

}
