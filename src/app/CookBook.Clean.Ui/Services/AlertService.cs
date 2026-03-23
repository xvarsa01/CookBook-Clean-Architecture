using CookBook.Clean.Ui.Services.Interfaces;

namespace CookBook.Clean.Ui.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        var page = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        await page.DisplayAlertAsync(title, message, "OK");
    }
}
