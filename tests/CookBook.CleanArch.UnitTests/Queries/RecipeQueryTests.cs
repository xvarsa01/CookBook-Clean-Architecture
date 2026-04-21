using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Application.Shared;

namespace CookBook.CleanArch.UnitTests.Queries;

public class RecipeQueryTests : QueryTestsBase
{
    [Fact]
    public async Task? Handle_FiltersByName_1()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "abcd" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Handle_FiltersByName_2()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "abcd" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Handle_FiltersByName_3()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "BCDEFG" };
        var paging = new PagingOptions();
        var query = new GetRecipeListQuery(filter, paging);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }
    
}
