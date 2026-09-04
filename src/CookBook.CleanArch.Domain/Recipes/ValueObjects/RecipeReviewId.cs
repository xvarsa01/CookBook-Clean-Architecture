using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record RecipeReviewId(Guid Value) : StronglyTypedId(Value);
