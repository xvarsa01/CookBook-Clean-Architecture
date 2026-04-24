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

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class CreateRecipeCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task CreateRecipeCommand_WithAllProperties_PersistsRecipe()
    {
        // Arrange
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients:
            [
                new RecipeCreateIngredientRequest(
                    IngredientTestSeeds.Lemon.Id,
                    IngredientAmount.CreateObject(200).Value,
                    MeasurementUnit.Pieces)
            ]
        );
        var command = new CreateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        await using var db = await DbContextFactory.CreateDbContextAsync();

        var recipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleAsync(r => r.Name == request.Name);

        Assert.Equal(request.Name, recipe.Name);
        Assert.Equal(request.Description, recipe.Description);
        Assert.Equal(request.ImageUrl!.Value, recipe.ImageUrl!.Value);

        Assert.Single(recipe.Ingredients);
        var ingredient = recipe.Ingredients.First();

        Assert.Equal(IngredientTestSeeds.Lemon.Id, ingredient.IngredientId);
        Assert.Equal(200, ingredient.Amount.Value);
        Assert.Equal(MeasurementUnit.Pieces, ingredient.Unit);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithEmptyIngredients_Returns_Failure()
    {
        // Arrange
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: []
        );
        var command = new CreateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNoIngredientsError(), result.Error);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .SingleOrDefaultAsync(r => r.Name == request.Name);
        Assert.Null(recipe);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithNullIngredients_Returns_Failure()
    {
        // Arrange
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: null
        );
        var command = new CreateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNoIngredientsError(), result.Error);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithNonExistingIngredient_Returns_Failure()
    {
        // Arrange
        var missingId = new IngredientId(Guid.NewGuid());
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients:
            [
                new RecipeCreateIngredientRequest(
                    missingId,
                    IngredientAmount.CreateObject(200).Value,
                    MeasurementUnit.Pieces)
            ]
        );

        var command = new CreateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(
            IngredientErrors.IngredientNotFoundError(missingId),
            result.Error);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithMoreThanMaximumIngredients_Returns_Failure()
    {
        // Arrange
        var ingredients = Enumerable.Range(0, 11)
            .Select(_ => new RecipeCreateIngredientRequest(
                IngredientTestSeeds.Lemon.Id,
                IngredientAmount.CreateObject(200).Value,
                MeasurementUnit.Pieces))
            .ToList();

        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: ingredients
        );

        var command = new CreateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("more than", result.Error.Message);
    }
}
