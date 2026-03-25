using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public record IngredientInRecipeId(Guid Id) : StronglyTypedId(Id);

