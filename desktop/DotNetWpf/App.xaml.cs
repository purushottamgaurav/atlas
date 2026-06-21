using DotNetWpf.Services;
using DotNetWpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DotNetWpf;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var collection = new ServiceCollection();
        ConfigureServices(collection);
        Services = collection.BuildServiceProvider();

        // Provide NavigationService with the built container
        var window = new MainWindow();
        window.DataContext = Services.GetRequiredService<ViewModels.MainViewModel>();
        window.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Infrastructure
        services.AddSingleton<SessionStore>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<ApiService>();
        services.AddSingleton<HubService>();

        // ViewModels — transient except MainViewModel which is effectively singleton via window
        services.AddSingleton<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LobbyViewModel>();
        services.AddTransient<RoomViewModel>();
        services.AddTransient<GameViewModel>();
        services.AddTransient<ResultsViewModel>();
    }
}
