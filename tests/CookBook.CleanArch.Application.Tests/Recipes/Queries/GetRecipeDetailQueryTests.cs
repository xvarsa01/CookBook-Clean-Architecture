using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Tests.Recipes.Queries;

public class GetRecipeDetailQueryTests : ApplicationTestsBase
{
    [Fact]
    public async Task Get_Recipe_Detail_Query_Returns_Result_When_Recipe_Exists()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var query = new GetRecipeDetailQuery(recipe.Id);
        
        // Act
        var result = await Mediator.Send(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        // Assert
        Assert.Equal(recipe.Id, result.Value.Id);
        Assert.Equal(recipe.Name, result.Value.Name);
        Assert.Equal(recipe.Description, result.Value.Description);
        Assert.Equal(recipe.ImageUrl, result.Value.ImageUrl);
        Assert.Equal(recipe.Duration, result.Value.Duration);
        Assert.Equal(recipe.Type, result.Value.Type);
        Assert.Equal(recipe.Ingredients.Count, result.Value.Ingredients.Count);
    }
    
    [Fact]
    public async Task Get_Recipe_Detail_Query_Returns_Result_With_All_Ingredients_When_Recipe_Exists()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var query = new GetRecipeDetailQuery(recipe.Id);
        
        // Act
        var result = await Mediator.Send(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        // Assert
        Assert.Equal(recipe.Ingredients.Count, result.Value.Ingredients.Count);

        var actualIngredientsById = result.Value.Ingredients.ToDictionary(x => x.Id);
        foreach (var expectedIngredientAmount in recipe.Ingredients)
        {
            Assert.True(actualIngredientsById.TryGetValue(expectedIngredientAmount.Id, out var actualIngredientAmount));
            Assert.NotNull(actualIngredientAmount);

            Assert.Equal(expectedIngredientAmount.Id, actualIngredientAmount.Id);
            Assert.Equal(expectedIngredientAmount.Amount, actualIngredientAmount.Amount);
            Assert.Equal(expectedIngredientAmount.Unit, actualIngredientAmount.Unit);
            Assert.Equal(expectedIngredientAmount.IngredientId, actualIngredientAmount.IngredientId);
            Assert.Equal(expectedIngredientAmount.Ingredient.Name, actualIngredientAmount.IngredientName);
        }
    }

    [Fact]
    public async Task Get_Recipe_Detail_Query_Returns_Failure_When_Recipe_Does_Not_Exist()
    {
        // Arrange
        var id = new RecipeId(Guid.NewGuid());
        var query = new GetRecipeDetailQuery(id);
        
        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains($"Recipe {id.Value} not found", result.Error.Message);
    }
}
