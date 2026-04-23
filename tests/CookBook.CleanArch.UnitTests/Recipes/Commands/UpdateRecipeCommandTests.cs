using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.UnitTests.Recipes.Commands;

public class UpdateRecipeCommandTests : UnitTestsBase
{
    [Fact]
    public async Task UpdateRecipeCommand_WithAllProperties_UpdatesRecipe()
    {
        // Arrange
        var existing = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var request = new RecipeUpdateRequest(
            Id: existing.Id,
            Name: RecipeName.CreateObject("updated name").Value,
            Description: "updated description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/updated.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(45)).Value,
            Type: RecipeType.Dessert
        );
        var command = new UpdateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(existing.Id, result.Value);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .SingleAsync(r => r.Id == existing.Id);

        Assert.Equal(request.Name, recipe.Name);
        Assert.Equal(request.Description, recipe.Description);
        Assert.Equal(request.ImageUrl!.Value, recipe.ImageUrl!.Value);
        Assert.Equal(request.Duration, recipe.Duration);
        Assert.Equal(request.Type, recipe.Type);
    }

    [Fact]
    public async Task UpdateRecipeCommand_WithPartialProperties_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var existing = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var originalDescription = existing.Description;
        var request = new RecipeUpdateRequest(
            Id: existing.Id,
            Name: RecipeName.CreateObject("updated name").Value,
            Description: null,
            ImageUrl: null,
            Duration: null,
            Type: null
        );
        var command = new UpdateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .SingleAsync(r => r.Id == existing.Id);

        Assert.Equal(request.Name, recipe.Name);
        Assert.Equal(originalDescription, recipe.Description);
    }

    [Fact]
    public async Task UpdateRecipeCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var id = new RecipeId(Guid.NewGuid());
        var request = new RecipeUpdateRequest(
            Id: id,
            Name: RecipeName.CreateObject("updated name").Value,
            Description: "desc",
            ImageUrl: null,
            Duration: null,
            Type: null
        );
        var command = new UpdateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(id), result.Error);
    }

    [Fact]
    public async Task UpdateRecipeCommand_WithNoChanges_Returns_SuccessAndDoesNotModify()
    {
        // Arrange
        var existing = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var request = new RecipeUpdateRequest(
            Id: existing.Id,
            Name: null,
            Description: null,
            ImageUrl: null,
            Duration: null,
            Type: null
        );
        var command = new UpdateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(existing.Id, result.Value);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .SingleAsync(r => r.Id == existing.Id);

        // nothing changed (basic sanity check)
        Assert.Equal(existing.Name, recipe.Name);
        Assert.Equal(existing.Description, recipe.Description);
    }

    [Fact]
    public async Task UpdateRecipeCommand_WithUpdatedDurationAndType_UpdatesRestFields()
    {
        // Arrange
        var existing = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);

        var request = new RecipeUpdateRequest(
            Id: existing.Id,
            Name: null,
            Description: null,
            ImageUrl: null,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(60)).Value,
            Type: RecipeType.Soup
        );

        var command = new UpdateRecipeCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();

        var recipe = await db.Recipes
            .SingleAsync(r => r.Id == existing.Id);

        Assert.Equal(request.Duration, recipe.Duration);
        Assert.Equal(request.Type, recipe.Type);
    }
}
