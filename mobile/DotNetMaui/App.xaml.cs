namespace DotNetMaui;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App(IServiceProvider services)
    {
        Services = services;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
