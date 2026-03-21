using CookBook.Clean.TestsBase;

namespace CookBook.Clean.CoreTests.RecipeRoot;

public class RecipeRootRemoveIngredientTests
{
        // Success removals
        
        [Fact]
        public void RemovingIngredient_From_Recipe_With_One_Ingredient_Should_Remove()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithSingleIngredient();
            var ingredient = RecipeTestSeeds.RecipeWithSingleIngredient().Ingredients.First();
            
            // Act
            recipe.RemoveIngredientByEntryId(recipe.Ingredients.First().Id);
            
            // Assert
            Assert.Empty(recipe.Ingredients);
            Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == ingredient.Id);
        }
        
        [Fact]
        public void RemovingIngredient_From_Recipe_With_Multiple_Ingredients_Should_Remove()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredient = RecipeTestSeeds.RecipeWithSingleIngredient().Ingredients.First();
            
            // Act
            recipe.RemoveIngredientByEntryId(recipe.Ingredients.First().Id);
            
            // Assert
            Assert.NotEmpty(recipe.Ingredients);
            Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == ingredient.Id);
        }
        
        [Fact]
        public void RemovingIngredients_From_Recipe_With_MultipleSame_Ingredients_Should_RemoveAllOfThem()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithDuplicateIngredientEntries();

            var ingredientId = IngredientTestSeeds.Lemon.Id;

            // Act
            recipe.RemoveIngredientsByIngredientId(ingredientId);

            // Assert
            Assert.NotEmpty(recipe.Ingredients); // water should remain
            Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == ingredientId);
        }
        
        [Fact]
        public void RemovingAllIngredients_From_Recipe_With_Multiple_Ingredients_Should_Remove_All()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            
            // Act
            recipe.RemoveAllIngredients();
            
            // Assert
            Assert.Empty(recipe.Ingredients);
        }
        
        
        // Failed removals
        
        [Fact]
        public void RemovingIngredient_From_Empty_Recipe_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.EmptyRecipe();
            var ingredient = IngredientTestSeeds.Water;
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                recipe.RemoveIngredientsByIngredientId(ingredient.Id)
            );
        }
        
        [Fact]
        public void RemovingAllIngredients_From_Empty_Recipe_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.EmptyRecipe();
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                recipe.RemoveAllIngredients()
            );
        }
        
        [Fact]
        public void RemovingIngredient_Not_Contained_In_Recipe_By_IngredientId_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                recipe.RemoveIngredientsByIngredientId(ingredient.Id)
            );
        }
        
        [Fact]
        public void RemovingIngredient_Not_Contained_In_Recipe_By_EntryId_Should_Throw()
        {
            // Arrange
            var recipe = RecipeTestSeeds.RecipeWithMultipleIngredients();
            var ingredient = IngredientTestSeeds.IngredientNotUsedInAnyRecipe;
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                recipe.RemoveIngredientByEntryId(ingredient.Id)
            );
        }
}
