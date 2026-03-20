namespace CookBook.Clean.Ui.Services.Interfaces;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
