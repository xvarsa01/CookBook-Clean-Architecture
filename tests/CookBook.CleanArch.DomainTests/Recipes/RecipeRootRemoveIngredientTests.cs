using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.DomainTests.RecipeRoot;

public class RecipeRootRemoveIngredientTests
{
    private static Recipe BuildRecipeWithSingleIngredient()
    {
        var ingredients = new List<RecipeCreateIngredient>
        {
            new(new IngredientId(Guid.NewGuid()), IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        return Recipe.Create(
            RecipeName.CreateObject("Recipe single ingredient").Value,
            null,
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            RecipeType.Other,
            ingredients).Value;
    }

    private static Recipe BuildRecipeWithMultipleIngredients()
    {
        var ingredientA = new IngredientId(Guid.NewGuid());
        var ingredientB = new IngredientId(Guid.NewGuid());
        var ingredients = new List<RecipeCreateIngredient>
        {
            new(ingredientA, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml),
            new(ingredientB, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Pieces)
        };

        return Recipe.Create(
            RecipeName.CreateObject("Recipe multiple ingredients").Value,
            null,
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            RecipeType.Other,
            ingredients).Value;
    }

    private static Recipe BuildRecipeWithDuplicateIngredientEntries(out IngredientId duplicatedIngredientId)
    {
        var ingredientA = new IngredientId(Guid.NewGuid());
        duplicatedIngredientId = new IngredientId(Guid.NewGuid());
        var ingredients = new List<RecipeCreateIngredient>
        {
            new(ingredientA, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml),
            new(duplicatedIngredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml),
            new(duplicatedIngredientId, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Pieces)
        };

        return Recipe.Create(
            RecipeName.CreateObject("Recipe duplicate ingredient").Value,
            null,
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            RecipeType.Other,
            ingredients).Value;
    }

    [Fact]
    public void RemovingIngredient_From_Recipe_With_One_Ingredient_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithSingleIngredient();
        var ingredientId = recipe.Ingredients.First().IngredientId;

        // Act
        var result = recipe.RemoveIngredientByEntryId(recipe.Ingredients.First().Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Single(recipe.Ingredients);
        Assert.Contains(recipe.Ingredients, i => i.IngredientId == ingredientId);
    }

    [Fact]
    public void RemovingIngredient_From_Recipe_With_Multiple_Ingredients_Should_Remove()
    {
        // Arrange
        var recipe = BuildRecipeWithMultipleIngredients();
        var ingredientId = recipe.Ingredients.First().IngredientId;

        // Act
        var result = recipe.RemoveIngredientByEntryId(recipe.Ingredients.First().Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(recipe.Ingredients);
        Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == ingredientId);
    }

    [Fact]
    public void RemovingIngredients_From_Recipe_With_MultipleSame_Ingredients_Should_RemoveAllOfThem()
    {
        // Arrange
        var recipe = BuildRecipeWithDuplicateIngredientEntries(out var ingredientId);

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(ingredientId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(recipe.Ingredients);
        Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == ingredientId);
    }

    [Fact]
    public void RemovingAllEntries_For_Only_Ingredient_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithSingleIngredient();
        var ingredientId = recipe.Ingredients.Single().IngredientId;

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(ingredientId);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RemovingIngredient_Not_Contained_In_Recipe_By_IngredientId_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithMultipleIngredients();
        var ingredientId = new IngredientId(Guid.NewGuid());

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(ingredientId);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RemovingIngredient_Not_Contained_In_Recipe_By_EntryId_Should_ReturnFailure()
    {
        // Arrange
        var recipe = BuildRecipeWithMultipleIngredients();
        var wrongEntryId = new RecipeIngredientId(Guid.NewGuid());

        // Act
        var result = recipe.RemoveIngredientByEntryId(wrongEntryId);

        // Assert
        Assert.True(result.IsFailure);
    }
}
