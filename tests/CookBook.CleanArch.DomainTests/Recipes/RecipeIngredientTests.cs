using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.DomainTests.Recipes;

public class RecipeIngredientTests
{
    private static Recipe BuildRecipeWithOneIngredient()
    {
        var ingredients = new List<RecipeCreateIngredient>
        {
            new(
                new IngredientId(Guid.NewGuid()),
                IngredientAmount.CreateObject(100).Value,
                MeasurementUnit.Ml)
        };

        return Recipe.Create(
            RecipeName.CreateObject("Recipe one ingredient").Value,
            null,
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            RecipeType.Other,
            ingredients).Value;
    }

    [Fact]
    public void AddingIngredients_With_Negative_Amount_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithOneIngredient();

        // Act & Assert
        var amountResult = IngredientAmount.CreateObject(-100);

        Assert.True(amountResult.IsFailure);
    }
    
    [Fact]
    public void AddingIngredients_With_Zero_Amount_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithOneIngredient();

        // Act & Assert
        var amountResult = IngredientAmount.CreateObject(0);

        Assert.True(amountResult.IsFailure);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Recipe_Should_Add()
    {
        // Arrange
        var recipe = BuildRecipeWithOneIngredient();
        var ingredientId = new IngredientId(Guid.NewGuid());
        
        // Act
        var result = recipe.AddIngredient(ingredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, recipe.Ingredients.Count);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_NonEmpty_Recipe_Should_Add()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var ingredientId = new IngredientId(Guid.NewGuid());
        
        // Act
        var result = recipe.AddIngredient(ingredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(recipe.Ingredients,  i => i.IngredientId == ingredientId);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Full_Recipe_Should_ReturnFailure()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeFullWithMaximumIngredients();
        var ingredientId = new IngredientId(Guid.NewGuid());
        
        // Act & Assert
        var result = recipe.AddIngredient(ingredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);

        Assert.True(result.IsFailure);
    }
}
