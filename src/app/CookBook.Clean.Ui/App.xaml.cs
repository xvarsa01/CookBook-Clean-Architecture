using CookBook.Clean.Ui.Shells;

namespace CookBook.Clean.Ui;

public partial class App
{
    private readonly IServiceProvider _serviceProvider;
    
    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var shell = _serviceProvider.GetRequiredService<AppShell>();
        return new Window(shell);
    }
}
