namespace CookBook.CleanArch.Domain.Shared;

public interface IValueObject<TValue>
{
    TValue Value { get; }
}

public interface IValueObjectFactory<TValueObject, in TValue>
{
    static abstract Result<TValueObject> CreateObject(TValue value);
}
