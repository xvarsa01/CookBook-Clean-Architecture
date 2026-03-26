using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Presentation.WebApi;

public class ValueObjectJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return IsValueObject(typeToConvert, out _);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, CreateConverter);
    }

    private static JsonConverter CreateConverter(Type typeToConvert)
    {
        if (!IsValueObject(typeToConvert, out var valueType))
        {
            throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");
        }

        var type = typeof(ValueObjectJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
        return (JsonConverter)Activator.CreateInstance(type)!;
    }

    private static bool IsValueObject(Type type, [NotNullWhen(true)] out Type? idType)
    {
        var baseType = type.BaseType;
        while (baseType != null)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(IValueObject<>))
            {
                idType = baseType.GetGenericArguments()[0];
                return true;
            }

            baseType = baseType.BaseType;
        }

        idType = null;
        return false;
    }
}
