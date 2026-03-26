using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Presentation.WebApi;

public class StronglyTypedIdJsonConverter<TStronglyTypedId> : JsonConverter<TStronglyTypedId>
    where TStronglyTypedId : StronglyTypedId
{
    private static readonly Func<Guid, TStronglyTypedId> Factory = CreateFactory();

    public override TStronglyTypedId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var id = JsonSerializer.Deserialize<Guid>(ref reader, options);
        return Factory(id);
    }

    public override void Write(Utf8JsonWriter writer, TStronglyTypedId? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value.Id, options);
    }

    private static Func<Guid, TStronglyTypedId> CreateFactory()
    {
        var ctor = typeof(TStronglyTypedId).GetConstructor([typeof(Guid)]);
        if (ctor is null)
        {
            throw new InvalidOperationException($"Type '{typeof(TStronglyTypedId)}' must have a public constructor with a single Guid parameter.");
        }

        var guid = Expression.Parameter(typeof(Guid), "id");
        var body = Expression.New(ctor, guid);
        return Expression.Lambda<Func<Guid, TStronglyTypedId>>(body, guid).Compile();
    }
}
