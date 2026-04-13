using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Moq;

namespace CookBook.CleanArch.UnitTests.UseCases.Ingredients;

public class IngredientUnitTests
{
    [Fact]
    public async Task CreateIngredientHandler_InsertsEntityAndReturnsId()
    {
        // Arrange
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        
        var expectedId = new IngredientId(Guid.NewGuid());
        repoMock.Setup(r => r.Add(It.IsAny<Ingredient>()))
            .Returns(expectedId);

        var handler = new CreateIngredientCommandHandler(repoMock.Object);
        var dto = new IngredientCreateRequest(
            "Sugar",
            "Sweet",
            ImageUrl.CreateObject("http://a.png").Value);
        var useCase = new CreateIngredientCommand(dto);

        // Act
        var result = await handler.Handle(useCase, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result.Value);
        repoMock.Verify(r => r.Add(It.Is<Ingredient>(e => e.Name == "Sugar" && e.ImageUrl != null && e.ImageUrl.Value == "http://a.png")), Times.Once);
    }

    [Fact]
    public async Task UpdateIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<IngredientId>())).ReturnsAsync((Ingredient?)null);

        var handler = new UpdateIngredientCommandHandler(repoMock.Object);
        var id = new IngredientId(Guid.NewGuid());
        var dto = new IngredientUpdateRequest(
            id,
            "New",
            "NewDesc",
            ImageUrl.CreateObject("http://a.png").Value);
        var useCase = new UpdateIngredientCommand(dto);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(
            IngredientErrors.IngredientNotFoundError(new IngredientId(id)),
            result.Error);
    }

    [Fact]
    public async Task CreateRecipeHandler_With_Initial_Ingredients_Should_Create_Recipe()
    {
        var recipeRepositoryMock = new Mock<IRepository<Recipe, RecipeId>>();
        var ingredientRepositoryMock = new Mock<IRepository<Ingredient, IngredientId>>();

        var expectedRecipeId = new RecipeId(Guid.NewGuid());
        recipeRepositoryMock.Setup(r => r.Add(It.IsAny<Recipe>())).Returns(expectedRecipeId);

        var existingIngredient = Ingredient.Create("Lemon", null, null).Value;
        ingredientRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<IngredientId>())).ReturnsAsync(existingIngredient);

        var handler = new CreateRecipeCommandHandler(recipeRepositoryMock.Object, ingredientRepositoryMock.Object);
        var request = new RecipeCreateRequest(
            RecipeName.CreateObject("Mojito").Value,
            "fresh drink",
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value,
            RecipeType.Drink,
            [
                new RecipeCreateIngredientRequest(new IngredientId(Guid.NewGuid()), IngredientAmount.CreateObject(2).Value, MeasurementUnit.Pieces),
                new RecipeCreateIngredientRequest(new IngredientId(Guid.NewGuid()), IngredientAmount.CreateObject(300).Value, MeasurementUnit.Ml)
            ]);

        var result = await handler.Handle(new CreateRecipeCommand(request), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedRecipeId, result.Value);
        recipeRepositoryMock.Verify(r => r.Add(It.Is<Recipe>(x => x.Ingredients.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task CreateRecipeHandler_Should_Return_NotFound_When_Ingredient_Does_Not_Exist()
    {
        var recipeRepositoryMock = new Mock<IRepository<Recipe, RecipeId>>();
        var ingredientRepositoryMock = new Mock<IRepository<Ingredient, IngredientId>>();

        ingredientRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<IngredientId>())).ReturnsAsync((Ingredient?)null);

        var handler = new CreateRecipeCommandHandler(recipeRepositoryMock.Object, ingredientRepositoryMock.Object);
        var missingIngredientId = new IngredientId(Guid.NewGuid());
        var request = new RecipeCreateRequest(
            RecipeName.CreateObject("Mojito").Value,
            "fresh drink",
            null,
            RecipeDuration.CreateObject(TimeSpan.FromMinutes(5)).Value,
            RecipeType.Drink,
            [new RecipeCreateIngredientRequest(missingIngredientId, IngredientAmount.CreateObject(2).Value, MeasurementUnit.Pieces)]);

        var result = await handler.Handle(new CreateRecipeCommand(request), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(missingIngredientId), result.Error);
        recipeRepositoryMock.Verify(r => r.Add(It.IsAny<Recipe>()), Times.Never);
    }
}
