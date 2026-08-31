using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.Commands;

public class RemoveIngredientsFromRecipeByIngredientIdCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly RemoveIngredientFromRecipeByIngredientIdCommandHandler _handler;

    public RemoveIngredientsFromRecipeByIngredientIdCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _handler = new RemoveIngredientFromRecipeByIngredientIdCommandHandler(_recipeRepositoryMock);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithSingleMatchingIngredient_RemovesIngredient()
    {
        // Arrange
        var ingredientToRemove = IngredientTestData.CreateIngredient();
        var ingredientToKeep = IngredientTestData.CreateIngredient();
        var recipe = RecipeTestData.CreateRecipe(
        ingredients: [
            new RecipeIngredientData(
                ingredientToRemove.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces),
            new RecipeIngredientData(
                ingredientToKeep.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ]);
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredientToRemove.Id);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var remainingIngredient = Assert.Single(recipe.Ingredients);
        Assert.Equal(ingredientToKeep.Id, remainingIngredient.IngredientId);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithMultipleMatchingIngredients_RemovesAllMatchingIngredients()
    {
        // Arrange
        var ingredientToRemove = IngredientTestData.CreateIngredient();
        var ingredientToKeep = IngredientTestData.CreateIngredient();
        var recipe = RecipeTestData.CreateRecipe(
        ingredients: [
            new RecipeIngredientData(
                ingredientToRemove.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces),
            new RecipeIngredientData(
                ingredientToRemove.Id,
                IngredientAmount.CreateObject(2).Value,
                MeasurementUnit.Pieces),
            new RecipeIngredientData(
                ingredientToKeep.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ]);
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredientToRemove.Id);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(recipe.Ingredients, ingredient => ingredient.IngredientId == ingredientToRemove.Id);
        Assert.Contains(recipe.Ingredients, ingredient => ingredient.IngredientId == ingredientToKeep.Id);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var ingredientId = IngredientTestData.CreateIngredient().Id;
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipeId, ingredientId);

        _recipeRepositoryMock.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task RemoveIngredientsFromRecipeByIngredientIdCommand_WithLastIngredient_ReturnsFailure()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var recipe = RecipeTestData.CreateRecipe(
        ingredients: [
            new RecipeIngredientData(
                ingredient.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ]);
        var originalIngredient = Assert.Single(recipe.Ingredients);
        var command = new RemoveIngredientsFromRecipeByIngredientIdCommand(recipe.Id, ingredient.Id);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);
        Assert.Same(originalIngredient, Assert.Single(recipe.Ingredients));
    }
}
