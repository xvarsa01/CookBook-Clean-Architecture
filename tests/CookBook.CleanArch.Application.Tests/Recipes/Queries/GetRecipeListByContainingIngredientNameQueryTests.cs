using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Common.Tests;

namespace CookBook.CleanArch.Application.Tests.Recipes.Queries;

public class GetRecipeListByContainingIngredientNameQueryTests : ApplicationTestsBase
{
    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientName_Query_With_Substring_Lem_Returns_Recipes_Containing_Lemon()
    {
        // Arrange
        var expectedRecipeIds = new[]
        {
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name).Id,
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithDuplicateIngredientEntries().Name).Id
        };

        var query = new GetRecipeListByContainingIngredientNameQuery("LeM");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedRecipeIds.Length, result.Value.Count);
        foreach (var expectedRecipeId in expectedRecipeIds)
        {
            Assert.Contains(result.Value, recipe => recipe.Id == expectedRecipeId);
        }
    }

    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientName_Query_With_Substring_AXA_Returns_Axa_Recipe()
    {
        // Arrange
        var expectedRecipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithUniqueIngredientOnly().Name);
        var query = new GetRecipeListByContainingIngredientNameQuery("used in single");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(expectedRecipe.Id, result.Value.Single().Id);
    }
}
