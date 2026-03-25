using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public record RecipeId(Guid Id) : StronglyTypedId(Id);

