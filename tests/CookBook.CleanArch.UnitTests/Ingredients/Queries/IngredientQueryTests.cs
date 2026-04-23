using CookBook.CleanArch.Application.Ingredients;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.UnitTests.Ingredients.Queries;

public class IngredientQueryTests : UnitTestsBase
{
    [Fact]
    public async Task Get_Ingredient_Detail_Query_Returns_Result_When_Ingredient_Exists()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.Water;
        var handler = new GetIngredientDetailQueryHandler(DbContext);
        var query = new GetIngredientDetailQuery(ingredient.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetIngredientDetailQueryHandler(DbContext);
        var query = new GetIngredientDetailQuery(id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains($"Ingredient {id.Value} not found", result.Error.Message);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Default_Paging_Returns_Max_10_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter();
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Items.Count() <= 10);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Unlimited_Paging_Returns_All_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageSize = int.MaxValue };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(IngredientTestSeeds.SeededIngredients.Count, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Specified_Paging_Returns_Specified_Number_Of_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter();
        var paging = new PagingOptions { PageSize = 7 };
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_X_Returns_Four_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {Name = "X"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_XXX_Returns_One_Matching_Result()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {Name = "XXX"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_X_Space_X_Returns_Two_Matching_Result()        // return also AXXXA
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {Name = "X X"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_Name_Filter_XXXX_Returns_No_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {Name = "XXXX"};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasDescription_True_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {HasDescription = true};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasDescription_False_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {HasDescription = false};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Items.Count() >= 7);
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_True_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {HasImage = true};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Ingredient_List_Query_With_HasImage_False_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetIngredientListQueryHandler(DbContext);
        var filter = new IngredientFilter {HasImage = false};
        var paging = new PagingOptions();
        var query = new GetIngredientListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
}
