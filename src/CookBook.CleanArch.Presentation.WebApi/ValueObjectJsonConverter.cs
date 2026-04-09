using System.Text.Json;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Presentation.WebApi;

public class ValueObjectJsonConverter<TValueObject, TValue> : System.Text.Json.Serialization.JsonConverter<TValueObject>
    where TValueObject : class, IValueObject<TValue>, IValueObjectFactory<TValueObject, TValue>
    where TValue : notnull
{
    public override TValueObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        var value = JsonSerializer.Deserialize<TValue>(ref reader, options);
        if (value is null)
        {
            return null;
        }

        var valueObjectResult = TValueObject.CreateObject(value);
        if (valueObjectResult.IsFailure)
        {
            throw new JsonException(valueObjectResult.Error.Message);
        }

        return valueObjectResult.Value;
    }

    public override void Write(Utf8JsonWriter writer, TValueObject? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value.Value, options);
    }
}
