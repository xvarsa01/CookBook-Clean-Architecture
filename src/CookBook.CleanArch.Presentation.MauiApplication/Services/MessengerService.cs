using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services;

public class MessengerService(IMessenger messenger) : IMessengerService
{
    public IMessenger Messenger { get; } = messenger;

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        Messenger.Send(message);
    }
}
