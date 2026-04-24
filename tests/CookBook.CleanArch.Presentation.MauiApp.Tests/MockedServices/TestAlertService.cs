using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;

public class TestAlertService : IAlertService
{
    public bool WasCalled { get; private set; }

    public Task DisplayAsync(string title, string message)
    {
        WasCalled = true;
        return Task.CompletedTask;
    }
}
