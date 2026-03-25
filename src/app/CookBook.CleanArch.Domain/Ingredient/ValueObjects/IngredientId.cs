using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Ingredient.ValueObjects;

public record IngredientId(Guid Id) : StronglyTypedId(Id);
