using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.TestsBase;

namespace CookBook.Clean.CoreTests.RecipeRoot;

public class RecipeRootAddIngredientTests
{
    [Fact]
    public void AddingIngredients_With_Negative_Amount_Should_Throw()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            recipe.AddIngredient(ingredient.Id, -100, MeasurementUnit.Ml)
        );
    }
    
    [Fact]
    public void AddingIngredients_With_Zero_Amount_Should_Throw()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            recipe.AddIngredient(ingredient.Id, 0, MeasurementUnit.Ml)
        );
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Empty_Recipe_Should_Add()
    {
        // Arrange
        var recipe = RecipeTestSeeds.EmptyRecipe();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act
        recipe.AddIngredient(ingredient.Id, 100, MeasurementUnit.Ml);
        
        // Assert
        Assert.Single(recipe.Ingredients);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_NonEmpty_Recipe_Should_Add()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act
        recipe.AddIngredient(ingredient.Id, 100, MeasurementUnit.Ml);
        
        // Assert
        Assert.Contains(recipe.Ingredients,  i => i.IngredientId == ingredient.Id);
    }
    
    [Fact]
    public void AddingIngredients_With_Positive_Amount_To_Full_Recipe_Should_Throw()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeFullWith10Ingredients();
        var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            recipe.AddIngredient(ingredient.Id, 100, MeasurementUnit.Ml)
        );
    }
}
