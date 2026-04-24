using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class DeleteRecipeCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task DeleteRecipeCommand_WithExistingRecipe_DeletesRecipe()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfDeleteWithIngredient().Name);
        var command = new DeleteRecipeCommand(recipe.Id);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();

        var deleted = await db.Recipes
            .SingleOrDefaultAsync(r => r.Id == recipe.Id);

        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteRecipeCommand_WithNonExistingRecipe_Returns_Failure()
    {
        // Arrange
        var id = new RecipeId(Guid.NewGuid());
        var command = new DeleteRecipeCommand(id);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(id), result.Error);
    }
}
