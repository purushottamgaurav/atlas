using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWpf.Services;

namespace DotNetWpf.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly SessionStore _session;
    private readonly NavigationService _nav;

    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _isRegisterMode;

    public LoginViewModel(ApiService api, SessionStore session, NavigationService nav)
    {
        _api = api;
        _session = session;
        _nav = nav;
    }

    [RelayCommand]
    private void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAsync()
    {
        ErrorMessage = string.Empty;
        IsBusy = true;
        try
        {
            var response = IsRegisterMode
                ? await _api.RegisterAsync(Username, Email, Password)
                : await _api.LoginAsync(Email, Password);

            _session.Token = response.Token;
            _session.UserId = response.UserId;
            _session.Username = response.Username;
            _session.Email = response.Email;

            _nav.NavigateTo<LobbyViewModel>();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanSubmit() => !IsBusy;
}
