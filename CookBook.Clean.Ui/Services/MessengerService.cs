using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.Ui.Services.Interfaces;

namespace CookBook.Clean.Ui.Services;

public class MessengerService(IMessenger messenger) : IMessengerService
{
    public IMessenger Messenger { get; } = messenger;

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        Messenger.Send(message);
    }
}
