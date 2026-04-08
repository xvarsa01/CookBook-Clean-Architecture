namespace CookBook.CleanArch.Domain.Shared;

public abstract record AggregateRootBase<TId>(TId Id) : EntityBase<TId>(Id) where TId : StronglyTypedId;
