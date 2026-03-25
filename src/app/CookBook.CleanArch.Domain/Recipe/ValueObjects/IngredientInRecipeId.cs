using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public record IngredientInRecipeId(Guid Id) : StronglyTypedId(Id);

