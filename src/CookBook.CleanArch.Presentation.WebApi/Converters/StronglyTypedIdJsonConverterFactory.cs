using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Presentation.WebApi.Converters;

public class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return !typeToConvert.IsAbstract && typeof(StronglyTypedId).IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, static type =>
        {
            var converterType = typeof(StronglyTypedIdJsonConverter<>).MakeGenericType(type);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}

