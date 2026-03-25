namespace CookBook.CleanArch.Domain.Shared;

public interface IStronglyTypedId;

public abstract record StronglyTypedId<T>(T Id) : IStronglyTypedId
{
    public override string ToString()
    {
        return Id?.ToString() ?? string.Empty;
    }

    public static implicit operator string(StronglyTypedId<T> stronglyTypedTypedId) => stronglyTypedTypedId.ToString();
}

public abstract record StronglyTypedId(Guid Id) : StronglyTypedId<Guid>(Id)
{
    public override string ToString() => base.ToString();

    public static implicit operator Guid(StronglyTypedId stronglyTypedTypedId) => stronglyTypedTypedId.Id;
}
