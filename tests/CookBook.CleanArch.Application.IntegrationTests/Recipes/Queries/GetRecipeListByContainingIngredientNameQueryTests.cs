using CookBook.CleanArch.Application.IntegrationTests.Infrastructure;
using CookBook.CleanArch.Application.Recipes.Queries;

namespace CookBook.CleanArch.Application.IntegrationTests.Recipes.Queries;

public class GetRecipeListByContainingIngredientNameQueryTests : BaseIntegrationTest
{
    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientName_Query_With_Substring_Lem_Returns_Recipes_Containing_Lemon()
    {
        // Arrange
        var expectedRecipeIds = new[]
        {
            Recipes.WithTwoIngredients.Id,
            Recipes.WithDuplicateIngredientEntries.Id
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
    public async Task Get_Recipe_List_By_Containing_IngredientName_Query_With_Water_Returns_All_Recipes()
    {
        // Arrange
        var query = new GetRecipeListByContainingIngredientNameQuery("water");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Recipes.All.Count, result.Value.Count);
        Assert.All(Recipes.All, recipe => Assert.Contains(result.Value, resultRecipe => resultRecipe.Id == recipe.Id));
    }
}
