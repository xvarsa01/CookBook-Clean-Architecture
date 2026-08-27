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

public class UpdateRecipeCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly IIngredientRepository _ingredientRepositoryMock;
    private readonly UpdateRecipeCommandHandler _handler;

    public UpdateRecipeCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _ingredientRepositoryMock = Substitute.For<IIngredientRepository>();
        _handler = new UpdateRecipeCommandHandler(_recipeRepositoryMock, _ingredientRepositoryMock);
    }

    [Fact]
    public async Task WithValidRecipeDataOnly_UpdatesRecipeAndPreservesIngredients()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var originalIngredientIds = recipe.Ingredients.Select(ingredient => ingredient.Id).ToList();
        var request = new RecipeUpdateWithIngredientsRequest(
            recipe.Id,
            RecipeName.CreateObject("updated name").Value,
            "updated description",
            ImageUrl.CreateObject("http://example.com/updated.png").Value,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            RecipeType.Soup,
            null);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(recipe.Id, result.Value);
        Assert.Equal(request.Name, recipe.Name);
        Assert.Equal(request.Description, recipe.Description);
        Assert.Equal(request.ImageUrl, recipe.ImageUrl);
        Assert.Equal(request.Duration, recipe.Duration);
        Assert.Equal(request.Type, recipe.Type);
        Assert.Equal(originalIngredientIds, recipe.Ingredients.Select(ingredient => ingredient.Id));
    }

    [Fact]
    public async Task WithDesiredIngredientAdded_AddsIngredient()
    {
        // Arrange
        var existingIngredient = IngredientTestData.CreateIngredient();
        var addedIngredient = IngredientTestData.CreateIngredient();
        var existingAmount = IngredientAmount.CreateObject(1).Value;
        var addedAmount = IngredientAmount.CreateObject(200).Value;
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(existingIngredient.Id, existingAmount, MeasurementUnit.Pieces)
        ]);
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(existingIngredient.Id, existingAmount, MeasurementUnit.Pieces),
            new(addedIngredient.Id, addedAmount, MeasurementUnit.Pieces)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(existingIngredient.Id).Returns(existingIngredient);
        _ingredientRepositoryMock.GetByIdAsync(addedIngredient.Id).Returns(addedIngredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, recipe.Ingredients.Count);
        Assert.Contains(recipe.Ingredients, ingredient => ingredient.IngredientId == addedIngredient.Id);
    }

    [Fact]
    public async Task WithDesiredIngredientChanged_UpdatesIngredient()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(
                ingredient.Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ]);
        var updatedAmount = IngredientAmount.CreateObject(500).Value;
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(ingredient.Id, updatedAmount, MeasurementUnit.Ml)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedIngredient = Assert.Single(recipe.Ingredients);
        Assert.Equal(updatedAmount, updatedIngredient.Amount);
        Assert.Equal(MeasurementUnit.Ml, updatedIngredient.Unit);
    }

    [Fact]
    public async Task WithDesiredIngredientRemoved_RemovesIngredient()
    {
        // Arrange
        var removedIngredient = IngredientTestData.CreateIngredient();
        var retainedIngredient = IngredientTestData.CreateIngredient();
        var amount = IngredientAmount.CreateObject(1).Value;
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(removedIngredient.Id, amount, MeasurementUnit.Pieces),
            new RecipeIngredientData(retainedIngredient.Id, amount, MeasurementUnit.Pieces)
        ]);
        var removedEntryId = recipe.Ingredients.Single(x => x.IngredientId == removedIngredient.Id).Id;
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(retainedIngredient.Id, amount, MeasurementUnit.Pieces)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(retainedIngredient.Id).Returns(retainedIngredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(recipe.Ingredients, ingredient => ingredient.Id == removedEntryId);
        Assert.Equal(retainedIngredient.Id, Assert.Single(recipe.Ingredients).IngredientId);
    }

    [Fact]
    public async Task WithComplexDesiredState_ReconcilesAllIngredientChanges()
    {
        // Arrange
        var removedIngredient = IngredientTestData.CreateIngredient();
        var retainedIngredient = IngredientTestData.CreateIngredient();
        var addedIngredient = IngredientTestData.CreateIngredient();
        var originalAmount = IngredientAmount.CreateObject(1).Value;
        var updatedAmount = IngredientAmount.CreateObject(250).Value;
        var addedAmount = IngredientAmount.CreateObject(100).Value;
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(removedIngredient.Id, originalAmount, MeasurementUnit.Pieces),
            new RecipeIngredientData(retainedIngredient.Id, originalAmount, MeasurementUnit.Pieces)
        ]);
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(retainedIngredient.Id, updatedAmount, MeasurementUnit.Ml),
            new(addedIngredient.Id, addedAmount, MeasurementUnit.Ml)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(retainedIngredient.Id).Returns(retainedIngredient);
        _ingredientRepositoryMock.GetByIdAsync(addedIngredient.Id).Returns(addedIngredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, recipe.Ingredients.Count);
        Assert.DoesNotContain(recipe.Ingredients, ingredient => ingredient.IngredientId == removedIngredient.Id);
        Assert.Equal(
            updatedAmount,
            recipe.Ingredients.Single(ingredient => ingredient.IngredientId == retainedIngredient.Id).Amount);
        Assert.Contains(recipe.Ingredients, ingredient => ingredient.IngredientId == addedIngredient.Id);
    }

    [Fact]
    public async Task WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var request = new RecipeUpdateWithIngredientsRequest(recipeId, null, null, null, null, null, null);

        _recipeRepositoryMock.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
        await _ingredientRepositoryMock.DidNotReceive().GetByIdAsync(Arg.Any<IngredientId>());
    }

    [Fact]
    public async Task WithNonExistingIngredient_ReturnsFailure()
    {
        // Arrange
        var existingIngredient = IngredientTestData.CreateIngredient();
        var missingIngredientId = new IngredientId(Guid.NewGuid());
        var amount = IngredientAmount.CreateObject(1).Value;
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(existingIngredient.Id, amount, MeasurementUnit.Pieces)
        ]);
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(existingIngredient.Id, amount, MeasurementUnit.Pieces),
            new(missingIngredientId, amount, MeasurementUnit.Ml)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(existingIngredient.Id).Returns(existingIngredient);
        _ingredientRepositoryMock.GetByIdAsync(missingIngredientId).Returns((Ingredient?)null);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(missingIngredientId), result.Error);
        Assert.Single(recipe.Ingredients);
        Assert.Equal(existingIngredient.Id, recipe.Ingredients.Single().IngredientId);
    }

    [Fact]
    public async Task WithMoreThanMaximumIngredients_ReturnsFailure()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var amount = IngredientAmount.CreateObject(1).Value;
        var recipe = RecipeTestData.CreateRecipe(
            Enumerable.Range(0, Recipe.MaxIngredients)
                .Select(_ => new RecipeIngredientData(ingredient.Id, amount, MeasurementUnit.Pieces))
                .ToList());
        var ingredients = Enumerable.Range(0, Recipe.MaxIngredients + 1)
            .Select(_ => new RecipeUpdateIngredientRequest(ingredient.Id, amount, MeasurementUnit.Pieces))
            .ToList();
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id), result.Error);
        Assert.Equal(Recipe.MaxIngredients, recipe.Ingredients.Count);
    }

    [Fact]
    public async Task WithNoIngredients_ReturnsFailure()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var originalIngredient = Assert.Single(recipe.Ingredients);
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, []);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);
        Assert.Same(originalIngredient, Assert.Single(recipe.Ingredients));
    }

    [Fact]
    public async Task ReplacingOnlyIngredient_Succeeds()
    {
        // Arrange
        var originalIngredient = IngredientTestData.CreateIngredient();
        var replacementIngredient = IngredientTestData.CreateIngredient();
        var amount = IngredientAmount.CreateObject(100).Value;
        var recipe = RecipeTestData.CreateRecipe(
        [
            new RecipeIngredientData(originalIngredient.Id, amount, MeasurementUnit.Pieces)
        ]);
        var ingredients = new List<RecipeUpdateIngredientRequest>
        {
            new(replacementIngredient.Id, amount, MeasurementUnit.Ml)
        };
        var request = new RecipeUpdateWithIngredientsRequest(recipe.Id, null, null, null, null, null, ingredients);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);
        _ingredientRepositoryMock.GetByIdAsync(replacementIngredient.Id).Returns(replacementIngredient);

        // Act
        var result = await _handler.Handle(new UpdateRecipeCommand(request), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(replacementIngredient.Id, Assert.Single(recipe.Ingredients).IngredientId);
    }
}
