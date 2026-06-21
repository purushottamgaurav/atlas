namespace DotNetMaui.Pages;

public partial class GamePage : ContentPage
{
    private readonly GamePageModel _vm;

    public GamePage(GamePageModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Attach();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.Detach();
    }
}
