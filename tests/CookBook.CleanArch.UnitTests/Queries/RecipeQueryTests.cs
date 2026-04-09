using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Queries;

namespace CookBook.CleanArch.UnitTests.Queries;

public class RecipeQueryTests : QueryTestsBase
{
    [Fact]
    public async Task? Handle_FiltersByName_1()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "abcd" };
        var query = new GetRecipeListQuery(filter);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Handle_FiltersByName_2()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "abcd" };
        var query = new GetRecipeListQuery(filter);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count());
    }
    
    [Fact]
    public async Task Handle_FiltersByName_3()
    {
        var handler = new GetRecipeListQueryHandler(DbContext);
        var filter = new RecipeFilter { Name = "BCDEFG" };
        var query = new GetRecipeListQuery(filter);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }
    
}
