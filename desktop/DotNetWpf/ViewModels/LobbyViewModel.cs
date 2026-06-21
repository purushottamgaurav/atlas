using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWpf.Models;
using DotNetWpf.Services;
using System.Collections.ObjectModel;

namespace DotNetWpf.ViewModels;

public partial class LobbyViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly HubService _hub;
    private readonly NavigationService _nav;
    private readonly SessionStore _session;

    [ObservableProperty] private ObservableCollection<RoomDto> _activeRooms = [];
    [ObservableProperty] private ObservableCollection<QuizSummaryDto> _quizzes = [];
    [ObservableProperty] private QuizSummaryDto? _selectedQuiz;
    [ObservableProperty] private string _joinCode = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _welcomeText = string.Empty;

    public LobbyViewModel(ApiService api, HubService hub, NavigationService nav, SessionStore session)
    {
        _api = api;
        _hub = hub;
        _nav = nav;
        _session = session;

        WelcomeText = $"Welcome, {session.Username}!";

        _hub.ErrorReceived += msg => StatusMessage = msg;
    }

    public async Task InitializeAsync()
    {
        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            var quizzes = await _api.GetQuizzesAsync();
            Quizzes = new ObservableCollection<QuizSummaryDto>(quizzes);

            var rooms = await _api.GetActiveRoomsAsync();
            ActiveRooms = new ObservableCollection<RoomDto>(rooms);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCreateRoom))]
    private async Task CreateRoomAsync()
    {
        if (SelectedQuiz == null) return;

        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            await _hub.ConnectAsync();

            var room = await _api.CreateRoomAsync(SelectedQuiz.QuizId);
            await _hub.JoinRoomAsync(room.Code);

            var roomVm = _nav.Get<RoomViewModel>();
            roomVm.Initialize(room, isHost: true);
            _nav.NavigateTo<RoomViewModel>();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanCreateRoom() => !IsBusy && SelectedQuiz != null;

    [RelayCommand(CanExecute = nameof(CanJoin))]
    private async Task JoinRoomAsync()
    {
        if (string.IsNullOrWhiteSpace(JoinCode)) return;

        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            await _hub.ConnectAsync();
            await _hub.JoinRoomAsync(JoinCode.Trim().ToUpperInvariant());

            var state = await _api.GetRoomAsync(JoinCode.Trim().ToUpperInvariant());
            var roomVm = _nav.Get<RoomViewModel>();
            roomVm.Initialize(state, isHost: false);
            _nav.NavigateTo<RoomViewModel>();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanJoin() => !IsBusy;

    [RelayCommand]
    private async Task JoinActiveRoomAsync(RoomDto room)
    {
        JoinCode = room.Code;
        await JoinRoomAsync();
    }

    [RelayCommand]
    private void Logout()
    {
        _session.Clear();
        _nav.NavigateTo<LoginViewModel>();
    }
}
