using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Domain.Recipes.Enums;

namespace CookBook.CleanArch.Application.Tests.Recipes.Queries;

public class GetRecipeListQueryTests : ApplicationTestsBase
{
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_BCD_Returns_Four_Matching_Results()
    {
        // Arrange
        var filter = new RecipeFilter { Name = "BCD" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_ABCD_Returns_Two_Matching_Result()
    {
        // Arrange
        var filter = new RecipeFilter { Name = "abcd" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_CD_Space_EF_Returns_One_Matching_Result()
    {
        // Arrange
        var filter = new RecipeFilter { Name = "cd ef" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_CDED_Returns_One_Matching_Result()
    {
        // Arrange
        var filter = new RecipeFilter { Name = "cdef" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_RecipeType_Filter_True_Returns_Matching_Results()
    {
        // Arrange
        var filter = new RecipeFilter { RecipeType = RecipeType.Caffe };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_RecipeType_Filter_True_Returns_Matching_Results2()
    {
        // Arrange
        var filter = new RecipeFilter { RecipeType = RecipeType.Soup };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_MinimalDuration_Filter_Set_Returns_Matching_Results()
    {
        // Arrange
        var filter = new RecipeFilter { MinimalDuration = TimeSpan.FromMinutes(30) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_MinimalDuration_Filter_Set_Returns_Matching_Results2()
    {
        // Arrange
        var filter = new RecipeFilter { MinimalDuration = TimeSpan.FromMinutes(31) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    [Fact]
    public async Task Get_Recipe_List_Query_With_MaximalDuration_Filter_Set_Returns_Matching_Results()
    {
        // Arrange
        var filter = new RecipeFilter { MaximalDuration = TimeSpan.FromMinutes(3) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Name_Ascending_Returns_Monotonic_CaseInsensitive_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        AssertMonotonic(items, (prev, current) =>
            string.Compare(prev.Name, current.Name, StringComparison.OrdinalIgnoreCase) <= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Name_Descending_Returns_Monotonic_CaseInsensitive_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        AssertMonotonic(items, (prev, current) =>
            string.Compare(prev.Name, current.Name, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Type_Ascending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Type, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].Type.CompareTo(recipesById[current.Id].Type) <= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Type_Descending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Type, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].Type.CompareTo(recipesById[current.Id].Type) >= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Duration_Ascending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Duration, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].Duration <= recipesById[current.Id].Duration);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Duration_Descending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Duration, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].Duration >= recipesById[current.Id].Duration);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_CreatedAt_Ascending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.CreatedAt, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].CreatedAt <= recipesById[current.Id].CreatedAt);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_CreatedAt_Descending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.CreatedAt, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            recipesById[prev.Id].CreatedAt >= recipesById[current.Id].CreatedAt);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_ModifiedAt_Ascending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.ModifiedAt, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            Nullable.Compare(recipesById[prev.Id].ModifiedAt, recipesById[current.Id].ModifiedAt) <= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_ModifiedAt_Descending_Returns_Monotonic_Order()
    {
        // Arrange
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.ModifiedAt, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        var items = result.Value.Items.ToList();
        var recipesById = SeededRecipes.ToDictionary(r => r.Id);
        AssertMonotonic(items, (prev, current) =>
            Nullable.Compare(recipesById[prev.Id].ModifiedAt, recipesById[current.Id].ModifiedAt) >= 0);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Unknown_Sort_Parameter_Falls_Back_To_Name_Ascending()
    {
        // Arrange
        var fallbackFilter = new RecipeFilter { SortParameter = (RecipeSortParameter)999, IsSortAscending = false };
        var fallbackPaging = new PagingOptions { PageSize = 100 };
        var fallbackQuery = new GetRecipeListQuery(fallbackFilter, fallbackPaging);

        var explicitNameAscFilter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = true };
        var explicitNameAscPaging = new PagingOptions { PageSize = 100 };
        var explicitNameAscQuery = new GetRecipeListQuery(explicitNameAscFilter, explicitNameAscPaging);

        // Act
        var fallbackResult = await Mediator.Send(fallbackQuery);
        var explicitNameAscResult = await Mediator.Send(explicitNameAscQuery);

        // Assert
        Assert.True(fallbackResult.IsSuccess);
        Assert.True(explicitNameAscResult.IsSuccess);
        Assert.Equal(
            explicitNameAscResult.Value.Items.Select(i => i.Id).ToList(),
            fallbackResult.Value.Items.Select(i => i.Id).ToList());
    }

    private static void AssertMonotonic(
        IReadOnlyList<RecipeListResponse> items,
        Func<RecipeListResponse, RecipeListResponse, bool> isOrdered)
    {
        for (var i = 1; i < items.Count; i++)
        {
            Assert.True(isOrdered(items[i - 1], items[i]));
        }
    }
}
