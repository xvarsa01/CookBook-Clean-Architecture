using CookBook.CleanArch.Presentation.MauiApplication.Models;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync(string route);
    Task GoToAsync(string route, IDictionary<string, object?> parameters);
    bool SendBackButtonPressed();
}
