using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Ingredient.ValueObjects;

public record IngredientId(Guid Id) : StronglyTypedId(Id), IValueObject<Guid>, IValueObjectFactory<IngredientId, Guid>
{
    public Guid Value => Id;
    
    public static Result<IngredientId> CreateObject()
    {
        return Result.Ok(new IngredientId((Guid.NewGuid())));
    }
    public static Result<IngredientId> CreateObject(Guid id)
    {
        return Result.Ok(new IngredientId(id));
    }
}
