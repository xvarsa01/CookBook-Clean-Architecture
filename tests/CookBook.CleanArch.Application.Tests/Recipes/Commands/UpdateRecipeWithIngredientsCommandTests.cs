using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

using RecipeAddIngredientRequest = CookBook.CleanArch.Application.Recipes.Models.RecipeUpdateWithIngredientsAddIngredientRequest;
using RecipeUpdateIngredientRequest = CookBook.CleanArch.Application.Recipes.Models.RecipeUpdateWithIngredientsUpdateIngredientRequest;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class UpdateRecipeWithIngredientsCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithValidRecipeDataOnly_UpdatesRecipe()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var request = CreateRequest(
            recipe.Id,
            RecipeName.CreateObject("updated name").Value,
            "updated description",
            ImageUrl.CreateObject("http://example.com/updated.png").Value,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            RecipeType.Soup);

        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

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
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(200).Value, MeasurementUnit.Pieces)
        };

        var request = CreateRequest(recipe.Id, additions: additions);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

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
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToUpdate = recipe.Ingredients.First().Id;

        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(entryIdToUpdate, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, updates: updates);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

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
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var request = CreateRequest(recipe.Id, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

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
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientIds = recipe.Ingredients.ToList();
        var entryIdToRemove = ingredientIds[0].Id;
        var entryIdToUpdate = ingredientIds[1].Id;

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(entryIdToUpdate, IngredientAmount.CreateObject(250).Value, MeasurementUnit.Ml)
        };
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, additions: additions, updates: updates, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.Equal(2, updatedRecipe.Ingredients.Count);
        Assert.DoesNotContain(entryIdToRemove, updatedRecipe.Ingredients.Select(i => i.Id));
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        var recipeId = new RecipeId(Guid.NewGuid());
        var request = CreateRequest(recipeId);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingIngredientToAdd_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var nonExistingIngredientId = new IngredientId(Guid.NewGuid());
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(nonExistingIngredientId, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, additions: additions);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(nonExistingIngredientId), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_ExceedingMaxIngredients_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeFullWithMaximumIngredients().Name);
        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, additions: additions);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_BelowMinIngredients_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var request = CreateRequest(recipe.Id, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithUpdateAndRemovalConflict_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entryIdToRemove = recipe.Ingredients.First().Id;

        var removals = new List<RecipeIngredientId> { entryIdToRemove };
        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(entryIdToRemove, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, updates: updates, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_WithNonExistingEntryIdToUpdate_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var nonExistingEntryId = new RecipeIngredientId(Guid.NewGuid());

        var updates = new List<RecipeUpdateIngredientRequest>
        {
            new(nonExistingEntryId, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml)
        };

        var request = CreateRequest(recipe.Id, updates: updates);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(nonExistingEntryId, recipe.Id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_UpdateNameWithNullRemovesName_Preserves()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var request = CreateRequest(recipe.Id);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes.SingleAsync(r => r.Id == recipe.Id);
        Assert.Equal(recipe.Name.Value, updatedRecipe.Name.Value);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_ReplaceIngredientWithAddAndRemove_Works()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientIds = recipe.Ingredients.ToList();
        var entryIdToRemove = ingredientIds[0].Id;

        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Pieces)
        };
        var removals = new List<RecipeIngredientId> { entryIdToRemove };

        var request = CreateRequest(recipe.Id, additions: additions, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.Equal(2, updatedRecipe.Ingredients.Count);
    }

    [Fact]
    public async Task UpdateRecipeWithIngredientsCommand_ReplaceOnlyIngredientWithAddAndRemove_Works()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var entryIdToRemove = recipe.Ingredients.Single().Id;

        var additions = new List<RecipeAddIngredientRequest>
        {
            new(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml)
        };
        var removals = new List<RecipeIngredientId> { entryIdToRemove };

        var request = CreateRequest(recipe.Id, additions: additions, removals: removals);
        var command = CreateCommand(request);

        var result = await Mediator.Send(command);

        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Id == recipe.Id);

        Assert.Single(updatedRecipe.Ingredients);
        Assert.DoesNotContain(entryIdToRemove, updatedRecipe.Ingredients.Select(i => i.Id));
    }

    private static RecipeUpdateWithIngredientsRequest CreateRequest(
        RecipeId id,
        RecipeName? name = null,
        string? description = null,
        ImageUrl? imageUrl = null,
        RecipeDuration? duration = null,
        RecipeType? type = null,
        IReadOnlyCollection<RecipeAddIngredientRequest>? additions = null,
        IReadOnlyCollection<RecipeUpdateIngredientRequest>? updates = null,
        IReadOnlyCollection<RecipeIngredientId>? removals = null)
        => new(
            id,
            name,
            description,
            imageUrl,
            duration,
            type,
            additions ?? [],
            updates ?? [],
            removals ?? []);

    private static UpdateRecipeWithIngredientsCommand CreateCommand(RecipeUpdateWithIngredientsRequest request)
        => new(request);
}

