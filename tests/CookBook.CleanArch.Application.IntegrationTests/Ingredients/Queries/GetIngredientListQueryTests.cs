using CookBook.CleanArch.Application.Ingredients;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Application.IntegrationTests.Infrastructure;
using CookBook.CleanArch.Application.Shared;

namespace CookBook.CleanArch.Application.IntegrationTests.Ingredients.Queries;

public class GetIngredientListQueryTests : BaseIntegrationTest
{
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Default_Paging_Returns_10_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.Items.Count());
        Assert.Equal(Ingredients.All.Count, result.Value.TotalItemsCount);
        Assert.Equal(0, result.Value.PageIndex);
        Assert.Equal(10, result.Value.PageSize);
    }

    [Fact]
    public async Task Get_Ingredient_List_Query_With_Second_Page_Returns_Remaining_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageIndex = 1 };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count - paging.PageSize, result.Value.Items.Count());
        Assert.Equal(Ingredients.All.Count, result.Value.TotalItemsCount);
        Assert.Equal(1, result.Value.PageIndex);
        Assert.Equal(10, result.Value.PageSize);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Unlimited_Paging_Returns_All_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Specified_Paging_Returns_Specified_Number_Of_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageSize = 2 };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_Lemon_Returns_One_Matching_Result()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "LEMON"};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_Without_Spaces_Returns_Unused_Ingredient()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "UnusedIngredient"};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Unknown_Name_Returns_No_Results()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "Chocolate"};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasDescription_True_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasDescription = true};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count(i => i.Description is not null), result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasDescription_False_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasDescription = false};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count(i => i.Description is null), result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_True_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasImage = true};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count(i => i.ImageUrl is not null), result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_False_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasImage = false};
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Ingredients.All.Count(i => i.ImageUrl is null), result.Value.Items.Count());
    }
}
