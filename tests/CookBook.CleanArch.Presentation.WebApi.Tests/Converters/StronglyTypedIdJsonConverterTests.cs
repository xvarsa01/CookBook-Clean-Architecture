using System.Text.Json;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.WebApi.Converters;

namespace CookBook.CleanArch.Presentation.WebApi.Tests.Converters;

public class StronglyTypedIdJsonConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
        return options;
    }

    [Fact]
    public void Serialize_RecipeId_Writes_Guid_String()
    {
        var recipeId = new RecipeId(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var json = JsonSerializer.Serialize(recipeId, CreateOptions());

        Assert.Equal($"\"{recipeId.Value}\"", json);
    }

    [Fact]
    public void Deserialize_RecipeId_Reads_Guid_String()
    {
        var id = Guid.Parse("22222222-2222-2222-2222-222222222222");

        var result = JsonSerializer.Deserialize<RecipeId>($"\"{id}\"", CreateOptions());

        Assert.NotNull(result);
        Assert.Equal(id, result.Value);
    }

    [Fact]
    public void Deserialize_Null_Returns_Null()
    {
        var result = JsonSerializer.Deserialize<RecipeId?>("null", CreateOptions());

        Assert.Null(result);
    }
}
