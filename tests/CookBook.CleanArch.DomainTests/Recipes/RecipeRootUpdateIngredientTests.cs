using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.DomainTests.Recipes;

public class RecipeRootUpdateIngredientTests
{
        // Success updates
      
        [Fact]
        public void UpdatingIngredient_With_Positive_Amount_To_Empty_Recipe_Should_Update()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
            var recipeIngredient = recipe.Ingredients.First();
        
            // Act
        var result = recipe.UpdateIngredientEntry(recipeIngredient.Id, IngredientAmount.CreateObject(123456).Value, MeasurementUnit.Kg);
        
            // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(123456, recipe.Ingredients.First().Amount.Value);
            Assert.Equal(MeasurementUnit.Kg, recipe.Ingredients.First().Unit);
        }
        
        // Failed updates
        
        [Fact]
        public void UpdatingIngredient_With_Zero_Amount_To_Empty_Recipe_Should_ReturnFailure()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act & Assert
        var amountResult = IngredientAmount.CreateObject(0);

        Assert.True(amountResult.IsFailure);
        }
        
        [Fact]
        public void UpdatingIngredient_With_Negative_Amount_To_Empty_Recipe_Should_ReturnFailure()
        {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var ingredientInRecipe = recipe.Ingredients.First();
        
        // Act & Assert
        var amountResult = IngredientAmount.CreateObject(-100);

        Assert.True(amountResult.IsFailure);
        }
        
        [Fact]
        public void UpdatingIngredient_With_NonExisting_EntryId_Should_ReturnFailure()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        
            // Act & Assert
        var result = recipe.UpdateIngredientEntry(new RecipeIngredientId(Guid.NewGuid()), IngredientAmount.CreateObject(123456).Value, MeasurementUnit.Kg);

        Assert.True(result.IsFailure);
        }
}
