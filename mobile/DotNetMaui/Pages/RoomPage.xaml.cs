namespace DotNetMaui.Pages;

public partial class RoomPage : ContentPage
{
    private readonly RoomPageModel _vm;

    public RoomPage(RoomPageModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await OnAppearingAsync();
    }

    private async Task OnAppearingAsync()
    {
        var services = ((App)Application.Current!).Services;
        var session = services.GetRequiredService<SessionStore>();
        var api = services.GetRequiredService<ApiService>();
        var hub = services.GetRequiredService<HubService>();

        try
        {
            var state = await api.GetRoomAsync(session.CurrentRoomCode);
            _vm.Initialize(state, session.IsRoomHost, hub);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.Unsubscribe();
    }
}
