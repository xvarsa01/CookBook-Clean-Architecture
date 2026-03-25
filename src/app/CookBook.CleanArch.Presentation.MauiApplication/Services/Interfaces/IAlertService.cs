namespace CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
