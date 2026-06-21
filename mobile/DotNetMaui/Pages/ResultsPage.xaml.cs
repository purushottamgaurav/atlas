namespace DotNetMaui.Pages;

public partial class ResultsPage : ContentPage
{
    private readonly ResultsPageModel _vm;

    public ResultsPage(ResultsPageModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Initialize();
    }
}
