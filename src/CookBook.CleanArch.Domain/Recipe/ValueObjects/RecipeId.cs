using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public record RecipeId(Guid Value) : StronglyTypedId(Value);
