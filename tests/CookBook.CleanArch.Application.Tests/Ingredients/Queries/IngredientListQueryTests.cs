using CookBook.CleanArch.Application.Ingredients;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Common.Tests;

namespace CookBook.CleanArch.Application.Tests.Ingredients.Queries;

public class IngredientListQueryTests : ApplicationTestsBase
{
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Default_Paging_Returns_Max_10_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Items.Count() <= 10);
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
        Assert.Equal(IngredientTestSeeds.SeededIngredients.Count, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Specified_Paging_Returns_Specified_Number_Of_Results()
    {
        // Arrange
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageSize = 7 };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_X_Returns_Four_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "X"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_XXX_Returns_One_Matching_Result()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "XXX"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_X_Space_X_Returns_Two_Matching_Result()        // return also AXXXA
    {
        // Arrange
        var filter = new IngredientFilter {Name = "X X"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_XXXX_Returns_No_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {Name = "XXXX"};
        var paging = new PagingOptions();
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
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasDescription_False_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasDescription = false};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Items.Count() >= 7);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_True_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasImage = true};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_False_Returns_Matching_Results()
    {
        // Arrange
        var filter = new IngredientFilter {HasImage = false};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
}
