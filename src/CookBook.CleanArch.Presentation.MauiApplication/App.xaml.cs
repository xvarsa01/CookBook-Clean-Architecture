using CookBook.CleanArch.Presentation.MauiApplication.Shells;
using Microsoft.Extensions.Hosting;

namespace CookBook.CleanArch.Presentation.MauiApplication;

public partial class App
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IReadOnlyCollection<IHostedService> _hostedServices;
    
    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _hostedServices = _serviceProvider.GetServices<IHostedService>().ToArray();
        
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        _ = StartHostedServicesAsync(); // fire and forget, log exceptions inside
        var shell = _serviceProvider.GetRequiredService<AppShell>();
        return new Window(shell);
    }
    
    private async Task StartHostedServicesAsync()
    {
        foreach (var hostedService in _hostedServices)
        {
            await hostedService.StartAsync(CancellationToken.None);
        }
    }
}
