using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.Tests.Ingredients.Commands;

public class DeleteIngredientCommandTests
{
    private readonly IIngredientRepository _ingredientRepositoryMock;
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly DeleteIngredientCommandHandler _handler;

    public DeleteIngredientCommandTests()
    {
        _ingredientRepositoryMock = Substitute.For<IIngredientRepository>();
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _handler = new DeleteIngredientCommandHandler(_ingredientRepositoryMock, _recipeRepositoryMock);
    }

    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientExists_RemovesIngredient()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var ingredientId = ingredient.Id;
        var command = new DeleteIngredientCommand(ingredientId);

        _ingredientRepositoryMock.GetByIdAsync(ingredientId).Returns(ingredient);
        _recipeRepositoryMock.GetRecipeCountByContainingIngredientId(ingredientId).Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _ingredientRepositoryMock.Received(1).Delete(ingredient);
    }

    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientDoesNotExist_ReturnsNotFoundFailure()
    {
        // Arrange
        var ingredientId = new IngredientId(Guid.NewGuid());
        var command = new DeleteIngredientCommand(ingredientId);

        _ingredientRepositoryMock.GetByIdAsync(ingredientId).Returns((Ingredient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(ingredientId), result.Error);

        _recipeRepositoryMock.DidNotReceive().GetRecipeCountByContainingIngredientId(Arg.Any<IngredientId>());
        _ingredientRepositoryMock.DidNotReceive().Delete(Arg.Any<Ingredient>());
    }

    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientIsUsed_ReturnsValidationFailure()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var ingredientId = ingredient.Id;
        var command = new DeleteIngredientCommand(ingredientId);

        _ingredientRepositoryMock.GetByIdAsync(ingredientId).Returns(ingredient);
        _recipeRepositoryMock.GetRecipeCountByContainingIngredientId(ingredientId).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(1), result.Error);
        
        _ingredientRepositoryMock.DidNotReceive().Delete(Arg.Any<Ingredient>());
    }
}
