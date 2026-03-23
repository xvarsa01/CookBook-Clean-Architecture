using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.TestsBase;

namespace CookBook.Clean.CoreTests.RecipeRoot;

public class RecipeRootAddIngredientTests
{
    [Fact]
    public void AddingIngredients_With_Negative_Amount_Should_ReturnFailure()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;

        // Act & Assert
        var amountResult = IngredientAmount.CreateObject(-100);

        Assert.True(amountResult.IsFailure);
    }
    
    [Fact]
    public void AddingIngredients_With_Zero_Amount_Should_ReturnFailure()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;

        // Act & Assert
        var amountResult = IngredientAmount.CreateObject(0);

        Assert.True(amountResult.IsFailure);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Empty_Recipe_Should_Add()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act
        var result = recipe.AddIngredient(ingredient.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(recipe.Ingredients);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_NonEmpty_Recipe_Should_Add()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act
        var result = recipe.AddIngredient(ingredient.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(recipe.Ingredients,  i => i.IngredientId == ingredient.Id);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Full_Recipe_Should_ReturnFailure()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeFullWith10Ingredients();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act & Assert
        var result = recipe.AddIngredient(ingredient.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);

        Assert.True(result.IsFailure);
    }
}
