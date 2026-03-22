using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
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
        recipe.UpdateIngredientEntry(ingredientInRecipe.Id, new IngredientAmount(123456), MeasurementUnit.Kg);
        
            // Assert
        Assert.Equal(123456, recipe.Ingredients.First().Amount.Value);
            Assert.Equal(MeasurementUnit.Kg, recipe.Ingredients.First().Unit);
        }
        
        // Failed removals
        
        [Fact]
        public void UpdatingIngredient_With_Zero_Amount_To_Empty_Recipe_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            recipe.UpdateIngredientEntry(ingredientInRecipe.Id, new IngredientAmount(0), MeasurementUnit.Kg)
            );
        }
        
        [Fact]
        public void UpdatingIngredient_With_Negative_Amount_To_Empty_Recipe_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredientInRecipe = recipe.Ingredients.First();
        
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                    recipe.UpdateIngredientEntry(ingredientInRecipe.Id, new IngredientAmount(-100), MeasurementUnit.Kg)
            );
        }
        
        [Fact]
        public void UpdatingIngredient_With_NonExisting_EntryId_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
        
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            recipe.UpdateIngredientEntry(Guid.NewGuid(), new IngredientAmount(123456), MeasurementUnit.Kg)
            );
        }
}
