using CommunityToolkit.Mvvm.ComponentModel;
using DotNetWpf.Services;

namespace DotNetWpf.ViewModels;

// Root ViewModel — owns the current page displayed in MainWindow's ContentControl.
// Navigation is driven by NavigationService raising the Navigated event.
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private object? _currentView;

    public MainViewModel(NavigationService nav)
    {
        nav.Navigated += vm => CurrentView = vm;
        nav.NavigateTo<LoginViewModel>();
    }
}
