using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public record RecipeIngredientId(Guid Value) : StronglyTypedId(Value);
