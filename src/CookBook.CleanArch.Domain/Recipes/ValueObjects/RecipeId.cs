using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record RecipeId(Guid Value) : StronglyTypedId(Value);
