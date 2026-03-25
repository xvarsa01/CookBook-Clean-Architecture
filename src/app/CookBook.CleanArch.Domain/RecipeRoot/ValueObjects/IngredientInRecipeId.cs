using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

public record IngredientInRecipeId(Guid Id) : StronglyTypedId(Id);

