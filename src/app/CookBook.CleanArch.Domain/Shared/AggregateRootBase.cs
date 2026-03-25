namespace CookBook.CleanArch.Domain.Shared;

public abstract record AggregateRootBase<TId> : EntityBase<TId> where TId : StronglyTypedId;
