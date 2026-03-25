namespace CookBook.Clean.Core.Shared;

public interface IEntity
{
    DateTime CreatedAt { get; set; }

    DateTime? ModifiedAt { get; set; }
}

public interface IEntity<out TId> : IEntity where TId : IStronglyTypedId
{
    TId Id { get; }
}
