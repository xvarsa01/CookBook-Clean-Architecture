using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record RecipeIngredientId(Guid Value) : StronglyTypedId(Value);
