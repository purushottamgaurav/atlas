namespace DotNetMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("room", typeof(Pages.RoomPage));
        Routing.RegisterRoute("game", typeof(Pages.GamePage));
        Routing.RegisterRoute("results", typeof(Pages.ResultsPage));
    }
}
