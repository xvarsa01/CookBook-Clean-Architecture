using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Ingredients.ValueObjects;

public record IngredientId(Guid Value) : StronglyTypedId(Value);
