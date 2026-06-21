using Microsoft.Extensions.DependencyInjection;

namespace DotNetWpf.Services;

// Decoupled navigation: ViewModels call Navigate<T>() without knowing about Views.
// MainViewModel listens to the Navigated event and sets CurrentView,
// which DataTemplates in App.xaml map to the correct UserControl.
public class NavigationService
{
    private readonly IServiceProvider _sp;

    public event Action<object>? Navigated;

    public NavigationService(IServiceProvider sp) => _sp = sp;

    public void NavigateTo<TViewModel>() where TViewModel : class
    {
        var vm = _sp.GetRequiredService<TViewModel>();
        Navigated?.Invoke(vm);
    }

    public TViewModel Get<TViewModel>() where TViewModel : class =>
        _sp.GetRequiredService<TViewModel>();
}
