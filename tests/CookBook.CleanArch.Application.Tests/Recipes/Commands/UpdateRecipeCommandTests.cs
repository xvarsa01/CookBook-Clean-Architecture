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

public class UpdateRecipeCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task WithValidRecipeDataOnly_UpdatesRecipeAndPreservesIngredients()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var originalIngredientIds = recipe.Ingredients.Select(x => x.Id).ToList();
        var request = CreateRequest(
            recipe.Id,
            RecipeName.CreateObject("updated name").Value,
            "updated description",
            ImageUrl.CreateObject("http://example.com/updated.png").Value,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            RecipeType.Soup);

        var result = await Mediator.Send(new UpdateRecipeCommand(request));

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updatedRecipe = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        Assert.Equal("updated name", updatedRecipe.Name.Value);
        Assert.Equal("updated description", updatedRecipe.Description);
        Assert.Equal(TimeSpan.FromMinutes(30), updatedRecipe.Duration.Value);
        Assert.Equal(originalIngredientIds, updatedRecipe.Ingredients.Select(x => x.Id));
    }

    [Fact]
    public async Task WithDesiredIngredientAdded_AddsIngredient()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var ingredients = IngredientsOf(recipe);
        ingredients.Add(NewIngredient(IngredientTestSeeds.Lemon.Id, 200, MeasurementUnit.Pieces));

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        Assert.Equal(2, updated.Ingredients.Count);
        Assert.Contains(updated.Ingredients, x => x.IngredientId == IngredientTestSeeds.Lemon.Id);
    }

    [Fact]
    public async Task WithDesiredIngredientChanged_UpdatesIngredient()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var entry = recipe.Ingredients.First();
        var ingredients = IngredientsOf(recipe);
        ingredients[0] = new(entry.IngredientId, Amount(500), MeasurementUnit.Ml);

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        var updatedEntry = updated.Ingredients.Single(x => x.IngredientId == entry.IngredientId);
        Assert.Equal(500, updatedEntry.Amount.Value);
        Assert.Equal(MeasurementUnit.Ml, updatedEntry.Unit);
    }

    [Fact]
    public async Task WithDesiredIngredientRemoved_RemovesIngredient()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var removedEntryId = recipe.Ingredients.First().Id;
        var ingredients = IngredientsOf(recipe);
        ingredients.RemoveAt(0);

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        Assert.Single(updated.Ingredients);
        Assert.DoesNotContain(updated.Ingredients, x => x.Id == removedEntryId);
    }

    [Fact]
    public async Task WithComplexDesiredState_ReconcilesAllIngredientChanges()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var retained = recipe.Ingredients.Last();
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(retained.IngredientId, Amount(250), MeasurementUnit.Ml),
            NewIngredient(IngredientTestSeeds.IngredientNotUsedInAnyRecipe.Id, 100, MeasurementUnit.Ml)
        };

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        Assert.Equal(2, updated.Ingredients.Count);
        Assert.Equal(250, updated.Ingredients.Single(x => x.IngredientId == retained.IngredientId).Amount.Value);
        Assert.Contains(updated.Ingredients, x => x.IngredientId == IngredientTestSeeds.IngredientNotUsedInAnyRecipe.Id);
    }

    [Fact]
    public async Task WithNonExistingRecipe_ReturnsFailure()
    {
        var recipeId = new RecipeId(Guid.NewGuid());

        var result = await Send(recipeId, null);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task WithNonExistingIngredient_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var missingIngredientId = new IngredientId(Guid.NewGuid());
        var ingredients = IngredientsOf(recipe);
        ingredients.Add(NewIngredient(missingIngredientId, 100, MeasurementUnit.Ml));

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(missingIngredientId), result.Error);
    }

    [Fact]
    public async Task WithMoreThanMaximumIngredients_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeFullWithMaximumIngredients().Name);
        var ingredients = IngredientsOf(recipe);
        ingredients.Add(NewIngredient(IngredientTestSeeds.Lemon.Id, 100, MeasurementUnit.Ml));

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task WithNoIngredients_ReturnsFailure()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);

        var result = await Send(recipe.Id, []);

        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);
    }

    [Fact]
    public async Task ReplacingOnlyIngredient_Succeeds()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            NewIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml)
        };

        var result = await Send(recipe.Id, ingredients);

        Assert.True(result.IsSuccess);
        await using var db = await DbContextFactory.CreateDbContextAsync();
        var updated = await db.Recipes.Include(x => x.Ingredients).SingleAsync(x => x.Id == recipe.Id);
        Assert.Single(updated.Ingredients);
        Assert.Equal(IngredientTestSeeds.Water.Id, updated.Ingredients.Single().IngredientId);
    }

    private async Task<CookBook.CleanArch.Domain.Result<RecipeId>> Send(
        RecipeId recipeId,
        IReadOnlyCollection<RecipeUpdateIngredientRequest>? ingredients)
        => await Mediator.Send(new UpdateRecipeCommand(CreateRequest(recipeId, ingredients: ingredients)));

    private static RecipeUpdateWithIngredientsRequest CreateRequest(
        RecipeId id,
        RecipeName? name = null,
        string? description = null,
        ImageUrl? imageUrl = null,
        RecipeDuration? duration = null,
        RecipeType? type = null,
        IReadOnlyCollection<RecipeUpdateIngredientRequest>? ingredients = null)
        => new(id, name, description, imageUrl, duration, type, ingredients);

    private static List<RecipeUpdateIngredientRequest> IngredientsOf(Recipe recipe)
        => recipe.Ingredients
            .Select(x => new RecipeUpdateIngredientRequest(x.IngredientId, x.Amount, x.Unit))
            .ToList();

    private static RecipeUpdateIngredientRequest NewIngredient(
        IngredientId ingredientId,
        decimal amount,
        MeasurementUnit unit)
        => new(ingredientId, Amount(amount), unit);

    private static IngredientAmount Amount(decimal value)
        => IngredientAmount.CreateObject(value).Value;
}

