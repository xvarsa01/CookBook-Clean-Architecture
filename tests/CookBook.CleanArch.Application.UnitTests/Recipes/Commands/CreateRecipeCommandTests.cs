using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.Commands;

public class CreateRecipeCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly IIngredientRepository _ingredientRepositoryMock;
    private readonly CreateRecipeCommandHandler _handler;

    public CreateRecipeCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _recipeRepositoryMock
            .Add(Arg.Any<Recipe>())
            .Returns(call => call.Arg<Recipe>().Id);

        _ingredientRepositoryMock = Substitute.For<IIngredientRepository>();
        _handler = new CreateRecipeCommandHandler(_recipeRepositoryMock, _ingredientRepositoryMock);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithAllProperties_AddsRecipeToRepository()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var recipeIngredientRequest = new RecipeCreateIngredientRequest(
            ingredient.Id,
            IngredientAmount.CreateObject(200).Value,
            MeasurementUnit.Pieces);
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients:
            [
                recipeIngredientRequest
            ]);
        var command = new CreateRecipeCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var addedRecipe = Arg.Is<Recipe>(recipe =>
            recipe.Id == result.Value &&
            recipe.Name == request.Name &&
            recipe.Description == request.Description &&
            recipe.ImageUrl == request.ImageUrl &&
            recipe.Duration == request.Duration &&
            recipe.Type == request.Type &&
            recipe.Ingredients.Count == 1 &&
            recipe.Ingredients.Single().IngredientId == ingredient.Id &&
            recipe.Ingredients.Single().Amount == recipeIngredientRequest.Amount &&
            recipe.Ingredients.Single().Unit == recipeIngredientRequest.Unit);
        _recipeRepositoryMock.Received(1).Add(addedRecipe);
    }

    [Fact]
    public async Task CreateRecipeCommand_WithEmptyIngredients_ReturnsFailure()
    {
        // Arrange
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: []);
        var command = new CreateRecipeCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(), result.Error);
        _recipeRepositoryMock.DidNotReceive().Add(Arg.Any<Recipe>());
    }

    [Fact]
    public async Task CreateRecipeCommand_WithNullIngredients_ReturnsFailure()
    {
        // Arrange
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: null);
        var command = new CreateRecipeCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(), result.Error);
        _recipeRepositoryMock.DidNotReceive().Add(Arg.Any<Recipe>());
    }

    [Fact]
    public async Task CreateRecipeCommand_WithNonExistingIngredient_ReturnsFailure()
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
            ]);
        var command = new CreateRecipeCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(missingId).Returns((Ingredient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(missingId), result.Error);
        _recipeRepositoryMock.DidNotReceive().Add(Arg.Any<Recipe>());
    }

    [Fact]
    public async Task CreateRecipeCommand_WithMoreThanMaximumIngredients_ReturnsFailure()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var ingredients = Enumerable.Range(0, Recipe.MaxIngredients + 1)
            .Select(_ => new RecipeCreateIngredientRequest(
                ingredient.Id,
                IngredientAmount.CreateObject(200).Value,
                MeasurementUnit.Pieces))
            .ToList();
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: ingredients);
        var command = new CreateRecipeCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(new RecipeId(Guid.Empty)).Code, result.Error.Code);
        _recipeRepositoryMock.DidNotReceive().Add(Arg.Any<Recipe>());
    }
}
