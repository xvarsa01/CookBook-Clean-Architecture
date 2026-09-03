using System.Net;
using System.Net.Http.Json;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Presentation.WebApi.FunctionalTests.Controllers;

public class IngredientControllerTests : WebApiTestsBase
{
    [Fact]
    public async Task GetById_Returns_Ok_When_Ingredient_Exists()
    {
        var seededIngredient = Ingredients.Lemon;

        var response = await Client.GetAsync($"/ingredient/{seededIngredient.Id.Value}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var ingredient = await response.Content.ReadFromJsonAsync<IngredientResponse>(Options);
        Assert.NotNull(ingredient);
        Assert.Equal(seededIngredient.Id, ingredient.Id);
    }

    [Fact]
    public async Task GetById_Returns_NotFound_When_Ingredient_Does_Not_Exist()
    {
        var response = await Client.GetAsync($"/ingredient/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllIngredients_Returns_At_Least_One_Ingredient()
    {
        var response = await Client.GetAsync("/ingredient");
        response.EnsureSuccessStatusCode();
        
        var ingredients = await response.Content.ReadFromJsonAsync<PagedResult<IngredientListResponse>>(Options);
        Assert.NotNull(ingredients);
        Assert.NotEmpty(ingredients.Items);
    }

    [Fact]
    public async Task GetAllIngredients_Returns_10_Ingredients_When_PageSize_Is_Not_Specified()
    {
        var response = await Client.GetAsync("/ingredient");
        response.EnsureSuccessStatusCode();

        var ingredients = await response.Content.ReadFromJsonAsync<PagedResult<IngredientListResponse>>(Options);

        Assert.NotNull(ingredients);
        Assert.Equal(0, ingredients.PageIndex);
        Assert.Equal(10, ingredients.PageSize);
        Assert.Equal(10, ingredients.Items.Count());
    }
    
    [Fact]
    public async Task GetAllIngredients_Returns_10_Ingredients_When_PageSize_Is_0()
    {
        var response = await Client.GetAsync("/ingredient?PageSize=0");
        response.EnsureSuccessStatusCode();

        var ingredients = await response.Content.ReadFromJsonAsync<PagedResult<IngredientListResponse>>(Options);

        Assert.NotNull(ingredients);
        Assert.Equal(0, ingredients.PageIndex);
        Assert.Equal(10, ingredients.PageSize);
        Assert.Equal(10, ingredients.Items.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task GetAllIngredients_Returns_Specified_Number_Of_Ingredients_When_PageSize_Is_Specified(int pageSize)
    {
        var response = await Client.GetAsync($"/ingredient?PageSize={pageSize}");
        response.EnsureSuccessStatusCode();

        var ingredients = await response.Content.ReadFromJsonAsync<PagedResult<IngredientListResponse>>(Options);

        Assert.NotNull(ingredients);
        Assert.Equal(0, ingredients.PageIndex);
        Assert.Equal(pageSize, ingredients.PageSize);
        Assert.Equal(pageSize, ingredients.Items.Count());
    }
    
    [Fact]
    public async Task Create_Returns_Ok_When_Request_Is_Valid()
    {
        var request = new IngredientCreateRequest(
            Name: "new ingredient",
            Description: $"new description",
            ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value);

        var response = await Client.PostAsJsonAsync("/ingredient", request, Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_Returns_BadRequest_When_Name_Is_empty()
    {
        var request = new IngredientCreateRequest(
            Name: string.Empty,
            Description: $"new description",
            ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value);

        var response = await Client.PostAsJsonAsync("/ingredient", request, Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Update_Returns_Ok_When_Request_Is_Valid()
    {
        var seededIngredient = Ingredients.Unused;
        
        var response = await Client.PutAsJsonAsync(
            "/ingredient",
            new IngredientUpdateRequest(
                Id: seededIngredient.Id,
                Name: "updated Name",
                Description: "updated description",
                ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value),
            Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns_BadRequest_When_Ingredient_Does_Not_Exist()
    {
        var response = await Client.PutAsJsonAsync(
            "/ingredient",
            new IngredientUpdateRequest(
                Id: new IngredientId(Guid.NewGuid()),
                Name: "update will fail",
                Description: "description",
                ImageUrl: null),
            Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_Returns_NoContent_When_Ingredient_Exists()
    {
        var seededIngredient = Ingredients.Unused;

        var response = await Client.DeleteAsync($"/ingredient/{seededIngredient.Id.Value}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var deleted = await Client.GetAsync($"/ingredient/{seededIngredient.Id.Value}");
        Assert.Equal(HttpStatusCode.NotFound, deleted.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_Returns_NotFound_When_Ingredient_Does_Not_Exist()
    {
        var response = await Client.DeleteAsync($"/ingredient/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
