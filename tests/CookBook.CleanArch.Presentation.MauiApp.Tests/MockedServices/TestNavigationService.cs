using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;

public class TestNavigationService : INavigationService
{
    public bool BackNavigationCalled { get; private set; }
    public bool Navigated { get; private set; }

    public IReadOnlyList<(string Route, Type ViewType)> Routes => [];

    IEnumerable<RouteModel> INavigationService.Routes { get; } = [];

    public Task GoToAsync(string route)
    {
        Navigated = true;
        return Task.CompletedTask;
    }

    public Task GoToAsync(string route, IDictionary<string, object?> parameters)
    {
        Navigated = true;
        return Task.CompletedTask;
    }

    public bool SendBackButtonPressed()
    {
        BackNavigationCalled = true;
        return true;
    }
}
