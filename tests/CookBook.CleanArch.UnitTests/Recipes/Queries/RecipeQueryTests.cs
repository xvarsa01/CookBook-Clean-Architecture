using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.UnitTests.Recipes.Queries;

public class RecipeQueryTests : UnitTestsBase
{
    // GetRecipeDetailQuery:
    
    [Fact]
    public async Task Get_Recipe_Detail_Query_Returns_Result_When_Recipe_Exists()
    {
        // Arrange
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);
        var handler = new GetRecipeDetailQueryHandler(DbContext);
        var query = new GetRecipeDetailQuery(recipe.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeDetailQueryHandler(DbContext);
        var query = new GetRecipeDetailQuery(recipe.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeDetailQueryHandler(DbContext);
        var query = new GetRecipeDetailQuery(id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains($"Recipe {id.Value} not found", result.Error.Message);
    }
    
    
    // GetRecipeListQuery:
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_BCD_Returns_Four_Matching_Results()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "BCD" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_ABCD_Returns_Two_Matching_Result()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "abcd" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_CD_Space_EF_Returns_One_Matching_Result()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "cd ef" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_Name_Filter_CDED_Returns_One_Matching_Result()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "cdef" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_RecipeType_Filter_True_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { RecipeType = RecipeType.Caffe };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_RecipeType_Filter_True_Returns_Matching_Results2()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { RecipeType = RecipeType.Soup };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_MinimalDuration_Filter_Set_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { MinimalDuration = TimeSpan.FromMinutes(30) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Get_Recipe_List_Query_With_MinimalDuration_Filter_Set_Returns_Matching_Results2()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { MinimalDuration = TimeSpan.FromMinutes(31) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }
    [Fact]
    public async Task Get_Recipe_List_Query_With_MaximalDuration_Filter_Set_Returns_Matching_Results()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { MaximalDuration = TimeSpan.FromMinutes(3) };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }

    [Fact]
    public async Task Get_Recipe_List_Query_With_Sort_By_Name_Ascending_Returns_Monotonic_CaseInsensitive_Order()
    {
        // Arrange
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Type, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Type, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Duration, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.Duration, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.CreatedAt, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.CreatedAt, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.ModifiedAt, IsSortAscending = true };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { SortParameter = RecipeSortParameter.ModifiedAt, IsSortAscending = false };
        var paging = new PagingOptions { PageSize = 100 };
        var query = new GetRecipeListQuery(filter, paging);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var fallbackHandler = new GetRecipeListQueryHandler(DbContext);
        var fallbackFilter = new RecipeFilter { SortParameter = (RecipeSortParameter)999, IsSortAscending = false };
        var fallbackPaging = new PagingOptions { PageSize = 100 };
        var fallbackQuery = new GetRecipeListQuery(fallbackFilter, fallbackPaging);

        var explicitNameAscHandler = new GetRecipeListQueryHandler(DbContext);
        var explicitNameAscFilter = new RecipeFilter { SortParameter = RecipeSortParameter.Name, IsSortAscending = true };
        var explicitNameAscPaging = new PagingOptions { PageSize = 100 };
        var explicitNameAscQuery = new GetRecipeListQuery(explicitNameAscFilter, explicitNameAscPaging);

        // Act
        var fallbackResult = await fallbackHandler.Handle(fallbackQuery, CancellationToken.None);
        var explicitNameAscResult = await explicitNameAscHandler.Handle(explicitNameAscQuery, CancellationToken.None);

        // Assert
        Assert.True(fallbackResult.IsSuccess);
        Assert.True(explicitNameAscResult.IsSuccess);
        Assert.Equal(
            explicitNameAscResult.Value.Items.Select(i => i.Id).ToList(),
            fallbackResult.Value.Items.Select(i => i.Id).ToList());
    }
    

    // GetRecipeListByContainingIngredientIdQuery:

    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientId_Query_With_Lemon_Returns_Recipes_Containing_Lemon()
    {
        // Arrange
        var lemon = GetSeededIngredientsByName(IngredientTestSeeds.Lemon.Name);
        var expectedRecipeIds = new[]
        {
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name).Id,
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithDuplicateIngredientEntries().Name).Id
        };

        var handler = new GetRecipeListByContainingIngredientIdQueryHandler(DbContext);
        var query = new GetRecipeListByContainingIngredientIdQuery(lemon.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var ingredient = GetSeededIngredientsByName(IngredientTestSeeds.IngredientNotUsedInAnyRecipe.Name);
        var handler = new GetRecipeListByContainingIngredientIdQueryHandler(DbContext);
        var query = new GetRecipeListByContainingIngredientIdQuery(ingredient.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    // GetRecipeListByContainingIngredientNameQuery

    [Fact]
    public async Task Get_Recipe_List_By_Containing_IngredientName_Query_With_Substring_Lem_Returns_Recipes_Containing_Lemon()
    {
        // Arrange
        var expectedRecipeIds = new[]
        {
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name).Id,
            GetSeededRecipeByName(RecipeTestSeeds.RecipeWithDuplicateIngredientEntries().Name).Id
        };

        var handler = new GetRecipeListByContainingIngredientNameQueryHandler(DbContext);
        var query = new GetRecipeListByContainingIngredientNameQuery("LeM");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var handler = new GetRecipeListByContainingIngredientNameQueryHandler(DbContext);
        var query = new GetRecipeListByContainingIngredientNameQuery("used in single");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(expectedRecipe.Id, result.Value.Single().Id);
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
