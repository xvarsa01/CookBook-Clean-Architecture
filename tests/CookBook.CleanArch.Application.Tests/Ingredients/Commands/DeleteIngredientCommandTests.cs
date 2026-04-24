using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Ingredients.Commands;

public class DeleteIngredientCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientExists_RemovesIngredient()
    {
        // Arrange
        var ingredientId = new IngredientId(IngredientTestSeeds.IngredientForTestOfDelete.Id);
        var command = new DeleteIngredientCommand(ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.DoesNotContain(
            await dbxAssert.Ingredients.ToListAsync(),
            i => i.Id == ingredientId);
    }
    
    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientDoesNotExist_ReturnsNotFoundFailure()
    {
        // Arrange
        var ingredientId = new IngredientId(Guid.NewGuid());
        var command = new DeleteIngredientCommand(ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(ingredientId), result.Error);
    }
    
    [Fact]
    public async Task DeleteIngredientCommand_WhenIngredientIsUsed_ReturnsValidationFailure()
    {
        // Arrange
        var ingredientId = new IngredientId(IngredientTestSeeds.UsedInSingleRecipe.Id);
        var command = new DeleteIngredientCommand(ingredientId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(1), result.Error);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var stillExisting = await dbxAssert.Ingredients
            .SingleOrDefaultAsync(i => i.Id == ingredientId);
        Assert.NotNull(stillExisting);
    }
}
