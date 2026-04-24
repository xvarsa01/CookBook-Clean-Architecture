using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Application.Tests.Ingredients.Queries;

public class GetIngredientDetailQueryTests : ApplicationTestsBase
{
    [Fact]
    public async Task Get_Ingredient_Detail_Query_Returns_Result_When_Ingredient_Exists()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.Water;
        var query = new GetIngredientDetailQuery(ingredient.Id);
        
        // Act
        var result = await Mediator.Send(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        // Assert
        Assert.Equal(ingredient.Id, result.Value.Id);
        Assert.Equal(ingredient.Name, result.Value.Name);
        Assert.Equal(ingredient.Description, result.Value.Description);
        Assert.Equal(ingredient.ImageUrl, result.Value.ImageUrl);
    }

    [Fact]
    public async Task Get_Ingredient_Detail_Query_Returns_Failure_When_Ingredient_Does_Not_Exist()
    {
        // Arrange
        var id = new IngredientId(Guid.NewGuid());
        var query = new GetIngredientDetailQuery(id);
        
        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains($"Ingredient {id.Value} not found", result.Error.Message);
    }
}
