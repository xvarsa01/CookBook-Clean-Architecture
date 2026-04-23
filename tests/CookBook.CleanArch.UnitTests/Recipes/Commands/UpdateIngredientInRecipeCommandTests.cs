using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.UnitTests.Recipes.Commands;

public class UpdateIngredientInRecipeCommandTests : UnitTestsBase
{
    [Fact]
    public async Task UpdateIngredientInRecipeCommand_WithValidData_UpdatesIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var ingredientEntry = recipe.Ingredients.First();
        var request = new RecipeUpdateIngredientRequest(
            EntryId: ingredientEntry.Id,
            NewAmount: IngredientAmount.CreateObject(500).Value,
            NewUnit: MeasurementUnit.Slice
        );
        var command = new UpdateIngredientInRecipeCommand(recipe.Id, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        var updatedIngredient = updated.Ingredients
            .Single(i => i.Id == ingredientEntry.Id);

        Assert.Equal(500, updatedIngredient.Amount.Value);
        Assert.Equal(MeasurementUnit.Slice, updatedIngredient.Unit);
    }

    [Fact]
    public async Task UpdateIngredientInRecipeCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var request = new RecipeUpdateIngredientRequest(
            EntryId: new RecipeIngredientId(Guid.NewGuid()),
            NewAmount: IngredientAmount.CreateObject(200).Value,
            NewUnit: MeasurementUnit.Pieces
        );
        var command = new UpdateIngredientInRecipeCommand(recipeId, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task UpdateIngredientInRecipeCommand_WithNonExistingEntryId_Returns_Failure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var nonExistingEntryId = new RecipeIngredientId(Guid.NewGuid());
        var request = new RecipeUpdateIngredientRequest(
            EntryId: nonExistingEntryId,
            NewAmount: IngredientAmount.CreateObject(200).Value,
            NewUnit: MeasurementUnit.Pieces
        );
        var command = new UpdateIngredientInRecipeCommand(recipe.Id, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(nonExistingEntryId, recipe.Id), result.Error);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var unchanged = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        // Ensure no modification happened
        var ingredient = unchanged.Ingredients.First();
        Assert.NotEqual(200, ingredient.Amount.Value);
    }

    [Fact]
    public async Task UpdateIngredientInRecipeCommand_WithSameValues_KeepsIngredientUnchanged()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var ingredientEntry = recipe.Ingredients.First();
        var request = new RecipeUpdateIngredientRequest(
            EntryId: ingredientEntry.Id,
            NewAmount: ingredientEntry.Amount,
            NewUnit: ingredientEntry.Unit
        );
        var command = new UpdateIngredientInRecipeCommand(recipe.Id, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        var ingredient = updated.Ingredients.Single(i => i.Id == ingredientEntry.Id);

        Assert.Equal(ingredientEntry.Amount, ingredient.Amount);
        Assert.Equal(ingredientEntry.Unit, ingredient.Unit);
    }
}
