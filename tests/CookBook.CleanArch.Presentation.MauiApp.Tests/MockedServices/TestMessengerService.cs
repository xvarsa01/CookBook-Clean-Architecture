using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;

public class TestMessengerService(IMessenger messenger) : IMessengerService
{
    public IMessenger Messenger { get; } = messenger;
    public List<object> SentMessages { get; } = [];

    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        SentMessages.Add(message);
        Messenger.Send(message);
    }
}
