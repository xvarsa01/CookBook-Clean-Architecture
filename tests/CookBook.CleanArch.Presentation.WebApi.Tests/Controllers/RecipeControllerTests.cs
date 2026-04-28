using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Presentation.WebApi.Tests.Controllers;

public class RecipeControllerTests : WebApiTestsBase
{
    [Fact]
    public async Task GetById_Returns_Ok_When_Recipe_Exists()
    {
        var seededRecipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name);

        var response = await Client.Value.GetAsync($"/recipe/{seededRecipe.Id.Value}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var recipe = await response.Content.ReadFromJsonAsync<RecipeResponse>(Options);
        Assert.NotNull(recipe);
        Assert.Equal(seededRecipe.Id, recipe.Id);
    }

    [Fact]
    public async Task GetById_Returns_NotFound_When_Recipe_Does_Not_Exist()
    {
        var response = await Client.Value.GetAsync($"/recipe/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllRecipes_Returns_At_Last_One_Recipe()
    {
        var response = await Client.Value.GetAsync("/recipe");
        response.EnsureSuccessStatusCode();
        
        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);
        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes.Items);
    }

    [Fact]
    public async Task GetAllRecipes_Returns_10_Recipes_When_PageSize_Is_Not_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.Equal(0, recipes.PageIndex);
        Assert.Equal(10, recipes.PageSize);
    }

    [Fact]
    public async Task GetAllRecipes_Returns_10_Recipes_When_PageSize_Is_0()
    {
        var response = await Client.Value.GetAsync($"/recipe?PageSize=0");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.Equal(0, recipes.PageIndex);
        Assert.Equal(10, recipes.PageSize);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(15)]
    public async Task GetAllRecipes_Returns_Specified_Number_Of_Recipes_When_PageSize_Is_Specified(int pageSize)
    {
        var response = await Client.Value.GetAsync($"/recipe?PageSize={pageSize}");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.Equal(0, recipes.PageIndex);
        Assert.Equal(pageSize, recipes.PageSize);
        Assert.Equal(pageSize, recipes.Items.Count());
    }

    [Fact]
    public async Task GetAllRecipes_Returns_Only_Matching_Recipes_When_Name_Is_Specified()
    {
        var seededRecipe =  RecipeTestSeeds.RecipeWithSingleIngredient();
        var response = await Client.Value.GetAsync($"/recipe?Name={seededRecipe.Name.Value}");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes.Items);
        Assert.Contains(recipes.Items, r => r.Name == seededRecipe.Name.Value);
    }

    [Fact]
    public async Task GetAllRecipes_Returns_Matching_Recipes_When_RecipeType_Is_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe?RecipeType={nameof(RecipeType.Caffe)}");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes.Items);
        Assert.All(recipes.Items, item => Assert.Equal(RecipeType.Caffe, item.RecipeType));
    }

    [Fact]
    public async Task GetAllRecipes_Returns_Only_Recipes_Above_MinimalDuration_When_MinimalDuration_Is_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe?MinimalDuration={ToQueryValue(TimeSpan.FromMinutes(30))}");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);
        
        Assert.NotNull(recipes);
        Assert.Equal(2, recipes.Items.Count());
    }

    [Fact]
    public async Task GetAllRecipes_Returns_Only_Recipes_At_Or_Below_MaximalDuration_When_MaximalDuration_Is_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe?MaximalDuration={ToQueryValue(TimeSpan.FromMinutes(3))}");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        Assert.Single(recipes.Items);

    }

    [Fact]
    public async Task GetAllRecipes_Returns_Recipes_In_Name_Order_When_SortParameter_Is_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe?SortParameter=Name&IsSortAscending=true");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        var items = recipes.Items.ToList();
        for (int i = 0; i < items.Count - 1; i++)
        {
            Assert.True(string.Compare(items[i].Name, items[i + 1].Name, StringComparison.OrdinalIgnoreCase) <= 0);
        }
    }
    
    [Fact]
    public async Task GetAllRecipes_Returns_Recipes_In_Name_Order_Reversed_When_SortParameter_Is_Specified()
    {
        var response = await Client.Value.GetAsync($"/recipe?SortParameter=Name&IsSortAscending=false");
        response.EnsureSuccessStatusCode();

        var recipes = await response.Content.ReadFromJsonAsync<PagedResult<RecipeListResponse>>(Options);

        Assert.NotNull(recipes);
        var items = recipes.Items.ToList();
        for (int i = 0; i < items.Count - 1; i++)
        {
            Assert.True(string.Compare(items[i].Name, items[i + 1].Name, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }

    [Fact]
    public async Task GetListByIngredientId_Returns_Ok_When_Ingredient_Id_Is_Valid()
    {
        var ingredient = IngredientTestSeeds.Lemon;

        var response = await Client.Value.GetAsync($"/recipe/ingredient/{ingredient.Id.Value}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<RecipeListResponse>>(Options);
        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes);
    }

    [Fact]
    public async Task GetListByIngredientId_Returns_NotFound_When_Ingredient_Id_Is_Malformed()
    {
        var response = await Client.Value.GetAsync("/recipe/ingredient/not-a-guid");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetListByIngredientName_Returns_Ok_When_Ingredient_Name_Is_Valid()
    {
        var ingredient = IngredientTestSeeds.Lemon;

        var response = await Client.Value.GetAsync($"/recipe/ingredient?ingredientNameSubstring={Uri.EscapeDataString(ingredient.Name)}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<RecipeListResponse>>(Options);
        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes);
    }

    [Fact]
    public async Task GetListByIngredientName_Returns_BadRequest_When_Query_Is_Missing()
    {
        var response = await Client.Value.GetAsync("/recipe/ingredient");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Create_Returns_Ok_When_Request_Is_Valid()
    {
        var ingredient = IngredientTestSeeds.Lemon;
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: $"new description",
            ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: [
                new RecipeCreateIngredientRequest(
                    ingredient.Id,
                    IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Pieces)
            ]);

        var response = await Client.Value.PostAsJsonAsync("/recipe", request, Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_Returns_BadRequest_When_Ingredients_Are_Missing()
    {
        var request = new RecipeCreateRequest(
            Name: RecipeName.CreateObject("new recipe").Value,
            Description: "description",
            ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value,
            Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(30)).Value,
            Type: RecipeType.MainDish,
            Ingredients: []);

        var response = await Client.Value.PostAsJsonAsync("/recipe", request, Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns_Ok_When_Request_Is_Valid()
    {
        var seededRecipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name);
        var updatedName = RecipeName.CreateObject("updated name").Value;
        
        var response = await Client.Value.PutAsJsonAsync(
            "/recipe",
            new RecipeUpdateRequest(
                Id: seededRecipe.Id,
                Name: updatedName,
                Description: "updated description",
                ImageUrl: ImageUrl.CreateObject($"https://example.com/1234.jpg").Value,
                Duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(45)).Value,
                Type: RecipeType.Soup),
            Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns_BadRequest_When_Recipe_Does_Not_Exist()
    {
        var updatedName = RecipeName.CreateObject("update will fail").Value;
        
        var response = await Client.Value.PutAsJsonAsync(
            "/recipe",
            new RecipeUpdateRequest(
                Id: new RecipeId(Guid.NewGuid()),
                Name: updatedName,
                Description: "description",
                ImageUrl: null,
                Duration: null,
                Type: null),
            Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_Returns_NoContent_When_Recipe_Exists()
    {
        var seededRecipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfDeleteWithIngredient().Name);

        var response = await Client.Value.DeleteAsync($"/recipe/{seededRecipe.Id.Value}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var deleted = await Client.Value.GetAsync($"/recipe/{seededRecipe.Id.Value}");
        Assert.Equal(HttpStatusCode.NotFound, deleted.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_Returns_NotFound_When_Recipe_Does_Not_Exist()
    {
        var response = await Client.Value.DeleteAsync($"/recipe/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddIngredient_Returns_Ok_When_Request_Is_Valid()
    {
        var seededRecipe = GetSeededRecipeByName(RecipeTestSeeds.MinimalisticRecipe().Name);
        var newIngredient = IngredientTestSeeds.MinimalisticIngredient;

        var response = await Client.Value.PostAsJsonAsync(
            $"/recipe/{seededRecipe.Id.Value}/ingredient",
            new RecipeAddIngredientRequest(
                IngredientId: newIngredient.Id,
                Amount: IngredientAmount.CreateObject(100).Value,
                Unit: MeasurementUnit.Pieces),
            Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var recipe = await GetRecipeByIdAsync(seededRecipe.Id);
        Assert.Contains(recipe.Ingredients, ingredient => ingredient.IngredientId == newIngredient.Id);
    }

    [Fact]
    public async Task AddIngredient_Returns_BadRequest_When_Recipe_Does_Not_Exist()
    {
        var newIngredient = IngredientTestSeeds.MinimalisticIngredient;
        var response = await Client.Value.PostAsJsonAsync(
            $"/recipe/{Guid.NewGuid()}/ingredient",
            new RecipeAddIngredientRequest(
                IngredientId: newIngredient.Id,
                Amount: IngredientAmount.CreateObject(100).Value,
                Unit: MeasurementUnit.Pieces),
            Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RemoveIngredient_Returns_NoContent_When_Ingredient_Entry_Exists()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientEntryId = recipe.Ingredients.First().Id;

        var response = await Client.Value.DeleteAsync($"/recipe/{recipe.Id.Value}/ingredient/{ingredientEntryId.Value}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedRecipe = await GetRecipeByIdAsync(recipe.Id);
        Assert.DoesNotContain(updatedRecipe.Ingredients, i => i.Id == ingredientEntryId);
    }

    [Fact]
    public async Task RemoveIngredient_Returns_BadRequest_When_Recipe_Does_Not_Exist()
    {
        var response = await Client.Value.DeleteAsync($"/recipe/{Guid.NewGuid()}/ingredient/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateIngredient_Returns_Ok_When_Request_Is_Valid()
    {
        var recipe = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithTwoIngredients().Name);
        var ingredientEntry = recipe.Ingredients.First();

        var response = await Client.Value.PutAsJsonAsync(
            $"/recipe/{recipe.Id.Value}/ingredient",
            new RecipeUpdateIngredientRequest(
                EntryId: ingredientEntry.Id,
                NewAmount: IngredientAmount.CreateObject(250).Value,
                NewUnit: MeasurementUnit.Slice),
            Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedRecipe = await GetRecipeByIdAsync(recipe.Id);
        var updatedIngredient = updatedRecipe.Ingredients.First(i => i.Id == ingredientEntry.Id);
        Assert.Equal(250, updatedIngredient.Amount.Value);
        Assert.Equal(MeasurementUnit.Slice, updatedIngredient.Unit);
    }

    [Fact]
    public async Task UpdateIngredient_Returns_BadRequest_When_Recipe_Does_Not_Exist()
    {
        var response = await Client.Value.PutAsJsonAsync(
            $"/recipe/{Guid.NewGuid()}/ingredient",
            new RecipeUpdateIngredientRequest(
                EntryId: new RecipeIngredientId(Guid.NewGuid()),
                NewAmount: IngredientAmount.CreateObject(250).Value,
                NewUnit: MeasurementUnit.Slice),
            Options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<RecipeResponse> GetRecipeByIdAsync(RecipeId recipeId)
    {
        var response = await Client.Value.GetAsync($"/recipe/{recipeId.Value}");
        response.EnsureSuccessStatusCode();

        var recipe = await response.Content.ReadFromJsonAsync<RecipeResponse>(Options);
        return recipe!;
    }

    private static string ToQueryValue(TimeSpan value) => 
        Uri.EscapeDataString(value.ToString("c", CultureInfo.InvariantCulture));
}
