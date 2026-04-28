using System.Text.Json;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.WebApi.Converters;

namespace CookBook.CleanArch.Presentation.WebApi.Tests.Converters;

public class ValueObjectJsonConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ValueObjectJsonConverterFactory());
        return options;
    }

    [Fact]
    public void Serialize_RecipeName_Writes_String_Value()
    {
        var recipeName = RecipeName.CreateObject("pasta").Value;

        var json = JsonSerializer.Serialize(recipeName, CreateOptions());

        Assert.Equal("\"pasta\"", json);
    }

    [Fact]
    public void Deserialize_RecipeName_Reads_String_Value()
    {
        var result = JsonSerializer.Deserialize<RecipeName>("\"pasta\"", CreateOptions());

        Assert.NotNull(result);
        Assert.Equal("pasta", result.Value);
    }

    [Fact]
    public void Deserialize_RecipeName_With_Invalid_Value_Throws_JsonException()
    {
        var exception = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<RecipeName>("\"ab\"", CreateOptions()));

        Assert.Contains("Recipe name must be at least 3 characters.", exception.Message);
    }

    [Fact]
    public void Serialize_And_Deserialize_IngredientAmount_RoundTrips_Value()
    {
        var amount = IngredientAmount.CreateObject(12.5m).Value;

        var json = JsonSerializer.Serialize(amount, CreateOptions());
        var result = JsonSerializer.Deserialize<IngredientAmount>(json, CreateOptions());

        Assert.Equal("12.5", json);
        Assert.NotNull(result);
        Assert.Equal(amount.Value, result.Value);
    }

    [Fact]
    public void Deserialize_Null_Returns_Null()
    {
        var result = JsonSerializer.Deserialize<RecipeName?>("null", CreateOptions());

        Assert.Null(result);
    }
}
