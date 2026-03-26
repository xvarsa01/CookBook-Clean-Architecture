// using CommunityToolkit.Mvvm.ComponentModel;

using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Application.Models;

public interface IModel
{
    StronglyTypedId Id { get; }
}
