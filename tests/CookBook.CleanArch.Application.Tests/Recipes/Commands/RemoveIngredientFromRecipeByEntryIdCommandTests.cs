using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class RemoveIngredientFromRecipeByEntryIdCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task RemoveIngredientFromRecipeByEntryIdCommand_WithValidData_RemovesIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryId = recipe.Ingredients.First().Id;
        var command = new RemoveIngredientFromRecipeByEntryIdCommand(recipe.Id, entryId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.DoesNotContain(updated.Ingredients, i => i.Id == entryId);
    }

    [Fact]
    public async Task RemoveIngredientFromRecipeByEntryIdCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var entryId = new RecipeIngredientId(Guid.NewGuid());
        var command = new RemoveIngredientFromRecipeByEntryIdCommand(recipeId, entryId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task RemoveIngredientFromRecipeByEntryIdCommand_WithNonExistingEntryId_Returns_Failure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var nonExistingEntryId = new RecipeIngredientId(Guid.NewGuid());
        var command = new RemoveIngredientFromRecipeByEntryIdCommand(recipe.Id, nonExistingEntryId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task RemoveIngredientFromRecipeByEntryIdCommand_WithLastIngredient_Returns_Failure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var entryId = recipe.Ingredients.First().Id;
        var command = new RemoveIngredientFromRecipeByEntryIdCommand(recipe.Id, entryId);

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

        // Ensure nothing was removed
        Assert.Single(updated.Ingredients);
        Assert.Equal(entryId, updated.Ingredients.First().Id);
    }
}
