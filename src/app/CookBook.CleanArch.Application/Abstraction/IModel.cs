// using CommunityToolkit.Mvvm.ComponentModel;

using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Application.Abstraction;

public interface IModel
{
    StronglyTypedId Id { get; }
}
