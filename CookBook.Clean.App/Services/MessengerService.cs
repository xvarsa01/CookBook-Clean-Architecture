using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Services.Interfaces;

namespace CookBook.Clean.App.Services;

public class MessengerService(IMessenger messenger) : IMessengerService
{
    public IMessenger Messenger { get; } = messenger;

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        Messenger.Send(message);
    }
}
