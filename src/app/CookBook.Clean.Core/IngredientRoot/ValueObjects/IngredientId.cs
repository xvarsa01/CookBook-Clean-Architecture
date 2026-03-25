using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.Core.IngredientRoot.ValueObjects;

public record IngredientId(Guid Id) : StronglyTypedId(Id);
