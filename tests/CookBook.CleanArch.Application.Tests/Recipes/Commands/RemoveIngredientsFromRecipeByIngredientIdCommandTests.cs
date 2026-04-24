using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class RemoveIngredientsFromRecipeByIngredientIdCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithSingleMatchingIngredient_RemovesIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientId = recipe.Ingredients.First().IngredientId;
        var initialCount = recipe.Ingredients.Count;
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();

        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.Equal(initialCount - 1, updated.Ingredients.Count);
        Assert.DoesNotContain(updated.Ingredients, i => i.IngredientId == ingredientId);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithMultipleMatchingIngredients_RemovesAllMatchingIngredients()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithDuplicateIngredientEntries().Name);
        var ingredientId = recipe.Ingredients.First(i => i.IngredientId == IngredientTestSeeds.Lemon.Id).IngredientId;
        var initialMatchingCount = recipe.Ingredients.Count(i => i.IngredientId == ingredientId);
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();

        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.DoesNotContain(updated.Ingredients, i => i.IngredientId == ingredientId);
        Assert.True(initialMatchingCount > 1);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var ingredientId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name).Ingredients.First().IngredientId;
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipeId, ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithLastIngredient_Returns_Failure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var ingredientId = recipe.Ingredients.First().IngredientId;
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(
            RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id),
            result.Error);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        // Ensure nothing changed
        Assert.Single(updated.Ingredients);
        Assert.Equal(ingredientId, updated.Ingredients.First().IngredientId);
    }
}
