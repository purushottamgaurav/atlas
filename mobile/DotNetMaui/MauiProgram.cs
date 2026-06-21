using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace DotNetMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Core services
        builder.Services.AddSingleton<SessionStore>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<HubService>();

        // Page models
        builder.Services.AddSingleton<LoginPageModel>();
        builder.Services.AddSingleton<LobbyPageModel>();
        builder.Services.AddTransient<RoomPageModel>();
        builder.Services.AddTransient<GamePageModel>();
        builder.Services.AddTransient<ResultsPageModel>();

        // Pages (Shell ShellContent pages as singletons, route pages as transient)
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LobbyPage>();
        builder.Services.AddTransient<RoomPage>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<ResultsPage>();

        return builder.Build();
    }
}
