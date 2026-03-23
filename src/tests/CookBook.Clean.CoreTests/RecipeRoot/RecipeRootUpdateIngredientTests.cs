using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.TestsBase;

namespace CookBook.Clean.CoreTests.RecipeRoot;

public class RecipeRootUpdateIngredientTests
{
        // Success updates
      
        [Fact]
        public void UpdatingIngredient_With_Positive_Amount_To_Empty_Recipe_Should_Add()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act
        var result = recipe.UpdateIngredientEntry(ingredientInRecipe.Id, IngredientAmount.CreateObject(123456).Value, MeasurementUnit.Kg);
        
            // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(123456, recipe.Ingredients.First().Amount.Value);
            Assert.Equal(MeasurementUnit.Kg, recipe.Ingredients.First().Unit);
        }
        
        // Failed removals
        
        [Fact]
        public void UpdatingIngredient_With_Zero_Amount_To_Empty_Recipe_Should_ReturnFailure()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act & Assert
        var amountResult = IngredientAmount.CreateObject(0);

        Assert.True(amountResult.IsFailure);
        }
        
        [Fact]
        public void UpdatingIngredient_With_Negative_Amount_To_Empty_Recipe_Should_ReturnFailure()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act & Assert
        var amountResult = IngredientAmount.CreateObject(-100);

        Assert.True(amountResult.IsFailure);
        }
        
        [Fact]
        public void UpdatingIngredient_With_NonExisting_EntryId_Should_ReturnFailure()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
        
            // Act & Assert
        var result = recipe.UpdateIngredientEntry(Guid.NewGuid(), IngredientAmount.CreateObject(123456).Value, MeasurementUnit.Kg);

        Assert.True(result.IsFailure);
        }
}
