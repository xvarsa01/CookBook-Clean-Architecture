namespace CookBook.Clean.Core.Shared;

public abstract record AggregateRootBase<TId> : EntityBase<TId> where TId : StronglyTypedId;
