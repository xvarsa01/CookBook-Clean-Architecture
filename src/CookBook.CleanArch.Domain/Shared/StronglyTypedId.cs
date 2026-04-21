namespace CookBook.CleanArch.Domain.Shared;

public interface IStronglyTypedId;

public abstract record StronglyTypedId<T>(T Value) : IStronglyTypedId
{
    public sealed override string ToString() => Value?.ToString() ?? string.Empty;

    public static implicit operator string(StronglyTypedId<T> stronglyTypedTypedId) => stronglyTypedTypedId.ToString();
}

public abstract record StronglyTypedId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static implicit operator Guid(StronglyTypedId stronglyTypedTypedId) => stronglyTypedTypedId.Value;
}
