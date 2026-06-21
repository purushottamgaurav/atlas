using DotNetWpf.ViewModels;
using System.Windows.Controls;

namespace DotNetWpf.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();

        // Wire PasswordBox since it can't bind directly (security restriction)
        PasswordBox.PasswordChanged += (_, _) =>
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = PasswordBox.Password;
        };
    }
}
