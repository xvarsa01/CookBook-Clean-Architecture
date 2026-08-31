using CookBook.CleanArch.Application.IntegrationTests.Infrastructure;
using CookBook.CleanArch.Application.Recipes.Queries;

namespace CookBook.CleanArch.Application.IntegrationTests.Recipes.Queries;

public class GetRecipeListByContainingIngredientIdQueryTests : BaseIntegrationTest
{
    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientId_Query_With_Lemon_Returns_Recipes_Containing_Lemon()
    {
        // Arrange
        var lemon = Ingredients.Lemon;
        var expectedRecipeIds = new[]
        {
            Recipes.WithTwoIngredients.Id,
            Recipes.WithDuplicateIngredientEntries.Id
        };

        var query = new GetRecipeListByContainingIngredientIdQuery(lemon.Id);

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
    public async Task Get_Recipe_List_By_Containing_IngredientId_Query_With_Ingredient_Used_In_No_Recipes_Returns_Empty_List()
    {
        // Arrange
        var ingredient = Ingredients.Unused;
        var query = new GetRecipeListByContainingIngredientIdQuery(ingredient.Id);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}
