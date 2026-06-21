using DotNetWpf.ViewModels;
using System.Windows.Controls;

namespace DotNetWpf.Views;

public partial class LobbyView : UserControl
{
    public LobbyView() => InitializeComponent();

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is LobbyViewModel vm)
            await vm.InitializeAsync();
    }
}
