namespace CookBook.CleanArch.Domain.Shared;

public interface IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
}

public interface IEntity<out TId> : IEntity where TId : IStronglyTypedId
{
    TId Id { get; }
}
