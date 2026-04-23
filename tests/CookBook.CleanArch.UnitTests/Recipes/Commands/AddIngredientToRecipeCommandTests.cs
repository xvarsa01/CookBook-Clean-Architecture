using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.UnitTests.Recipes.Commands;

public class AddIngredientToRecipeCommandTests : UnitTestsBase
{
    [Fact]
    public async Task AddIngredientToRecipeCommand_WithValidData_AddsIngredient()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var request = new RecipeAddIngredientRequest(
            IngredientId: IngredientTestSeeds.Lemon.Id,
            Amount: IngredientAmount.CreateObject(200).Value,
            Unit: MeasurementUnit.Pieces
        );
        var command = new AddIngredientToRecipeCommand(recipeId, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipeId);
        
        Assert.Equal(2, recipe.Ingredients.Count);
    }

    [Fact]
    public async Task AddIngredientToRecipeCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var request = new RecipeAddIngredientRequest(
            IngredientId: IngredientTestSeeds.Lemon.Id,
            Amount: IngredientAmount.CreateObject(200).Value,
            Unit: MeasurementUnit.Pieces
        );
        var command = new AddIngredientToRecipeCommand(recipeId, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task AddIngredientToRecipeCommand_WithNonExistingIngredient_Returns_Failure()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var request = new RecipeAddIngredientRequest(
            IngredientId: new IngredientId(Guid.NewGuid()),
            Amount: IngredientAmount.CreateObject(200).Value,
            Unit: MeasurementUnit.Pieces
        );
        var command = new AddIngredientToRecipeCommand(recipeId, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(request.IngredientId), result.Error);
    }

    [Fact]
    public async Task AddIngredientToRecipeCommand_WithMaximumIngredientsReached_Returns_Failure()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeFullWithMaximumIngredients().Name).Id;
        var request = new RecipeAddIngredientRequest(
            IngredientId: IngredientTestSeeds.Lemon.Id,
            Amount: IngredientAmount.CreateObject(200).Value,
            Unit: MeasurementUnit.Pieces
        );
        var command = new AddIngredientToRecipeCommand(recipeId, request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipeId), result.Error);
    }
}
