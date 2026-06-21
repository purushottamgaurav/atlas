namespace DotNetMaui.Pages;

public partial class LobbyPage : ContentPage
{
    public LobbyPage(LobbyPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LobbyPageModel)BindingContext).InitializeAsync();
    }
}
