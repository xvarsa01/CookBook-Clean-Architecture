using CommunityToolkit.Mvvm.Messaging;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}
