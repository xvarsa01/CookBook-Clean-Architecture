using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public record RecipeId(Guid Id) : StronglyTypedId(Id), IValueObject<Guid>, IValueObjectFactory<RecipeId, Guid>
{
    public Guid Value => Id;
    
    public static Result<RecipeId> CreateObject()
    {
        return Result.Ok(new RecipeId((Guid.NewGuid())));
    }
    public static Result<RecipeId> CreateObject(Guid id)
    {
        return Result.Ok(new RecipeId(id));
    }
}

