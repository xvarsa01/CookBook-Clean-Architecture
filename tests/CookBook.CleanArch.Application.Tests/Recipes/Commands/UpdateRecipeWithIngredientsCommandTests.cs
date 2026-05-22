using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class UpdateRecipeWithIngredientsCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithValidRecipeDataOnly_UpdatesRecipe()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var request = new RecipeUpdateRequest(
            recipe.Id,
            RecipeName.CreateObject("updated name").Value,
            "updated description",
            ImageUrl.CreateObject("http://example.com/updated.png").Value,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            RecipeType.Soup);

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(recipe.Id, result.Value);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes.SingleAsync(r => r.Id == recipe.Id);
        Assert.Equal("updated name", updatedRecipe.Name.Value);
        Assert.Equal("updated description", updatedRecipe.Description);
        Assert.Equal("http://example.com/updated.png", updatedRecipe.ImageUrl?.Value);
        Assert.Equal(TimeSpan.FromMinutes(30), updatedRecipe.Duration.Value);
        Assert.Equal(RecipeType.Soup, updatedRecipe.Type);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithAdditionOnly_AddsIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var request = new RecipeUpdateRequest(
            recipe.Id, null, null, null, null, null);

        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(200).Value, MeasurementUnit.Pieces)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: additions,
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);
        
        Assert.Equal(2, updatedRecipe.Ingredients.Count);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithUpdateOnly_UpdatesIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToUpdate = recipe.Ingredients.First().Id;
        
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(entryIdToUpdate, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: updates,
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);
        
        var updatedIngredient = updatedRecipe.Ingredients.First(i => i.Id == entryIdToUpdate);
        Assert.Equal(500, updatedIngredient.Amount.Value);
        Assert.Equal(MeasurementUnit.Ml, updatedIngredient.Unit);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithRemovalOnly_RemovesIngredient()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;
        
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var removals = new List<RecipeIngredientId> { entryIdToRemove };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: [],
            Removals: removals);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);
        
        Assert.Single(updatedRecipe.Ingredients);
        Assert.DoesNotContain(entryIdToRemove, updatedRecipe.Ingredients.Select(i => i.Id));
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithComplexChanges_AppliesAllChanges()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientIds = recipe.Ingredients.ToList();
        var entryIdToRemove = ingredientIds[0].Id;
        var entryIdToUpdate = ingredientIds[1].Id;

        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(entryIdToUpdate, IngredientAmount.CreateObject(250).Value, MeasurementUnit.Ml)
        };
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: additions,
            Updates: updates,
            Removals: removals);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);
        
        // Should have 2 ingredients (removed 1, added 1, kept 1)
        Assert.Equal(2, updatedRecipe.Ingredients.Count);
        Assert.DoesNotContain(entryIdToRemove, updatedRecipe.Ingredients.Select(i => i.Id));
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var request = new RecipeUpdateRequest(recipeId, null, null, null, null, null);

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingIngredientToAdd_ReturnsFailure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var nonExistingIngredientId = new IngredientId(Guid.NewGuid());
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(nonExistingIngredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: additions,
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(nonExistingIngredientId), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_ExceedingMaxIngredients_ReturnsFailure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeFullWithMaximumIngredients().Name);
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: additions,
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_BelowMinIngredients_ReturnsFailure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;
        
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var removals = new List<RecipeIngredientId> { entryIdToRemove };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: [],
            Removals: removals);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithUpdateAndRemovalConflict_ReturnsFailure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;
        
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var updates = new List<RecipeUpdateIngredientRequest>
        {
            // Attempting to update an entry that is being removed
            new(entryIdToRemove, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: updates,
            Removals: removals);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingEntryIdToUpdate_ReturnsFailure()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var nonExistingEntryId = new RecipeIngredientId(Guid.NewGuid());
        
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(nonExistingEntryId, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: updates,
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(nonExistingEntryId, recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_UpdateNameWithNullRemovesName_Preserves()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: [],
            Updates: [],
            Removals: []);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes.SingleAsync(r => r.Id == recipe.Id);
        Assert.Equal(recipe.Name.Value, updatedRecipe.Name.Value);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_ReplaceIngredientWithAddAndRemove_Works()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientIds = recipe.Ingredients.ToList();
        var entryIdToRemove = ingredientIds[0].Id;

        var request = new RecipeUpdateRequest(recipe.Id, null, null, null, null, null);

        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Pieces)
        };
        var removals = new List<RecipeIngredientId> { entryIdToRemove };

        var command = new UpdateRecipeWithIngredientsCommand(
            Request: request,
            Additions: additions,
            Updates: [],
            Removals: removals);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);
        
        // Should still have 2 ingredients
        Assert.Equal(2, updatedRecipe.Ingredients.Count);
    }
}

