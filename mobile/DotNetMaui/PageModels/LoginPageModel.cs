using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DotNetMaui.PageModels;

public partial class LoginPageModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly SessionStore _session;

    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private bool isRegisterMode;

    public LoginPageModel(ApiService api, SessionStore session)
    {
        _api = api;
        _session = session;
    }

    [RelayCommand]
    private void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsBusy) return;
        ErrorMessage = string.Empty;
        IsBusy = true;
        try
        {
            AuthResponse result;
            if (IsRegisterMode)
            {
                result = await _api.RegisterAsync(new RegisterRequest
                {
                    Username = Username,
                    Email = Email,
                    Password = Password
                });
            }
            else
            {
                result = await _api.LoginAsync(new LoginRequest
                {
                    Email = Email,
                    Password = Password
                });
            }

            _session.Token = result.Token;
            _session.UserId = result.UserId;
            _session.Username = result.Username;
            _session.Email = result.Email;

            await Shell.Current.GoToAsync("//lobby");
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
}
