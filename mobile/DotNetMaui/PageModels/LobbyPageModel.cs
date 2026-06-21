using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DotNetMaui.PageModels;

public partial class LobbyPageModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly HubService _hub;
    private readonly SessionStore _session;

    [ObservableProperty] private ObservableCollection<RoomDto> activeRooms = [];
    [ObservableProperty] private ObservableCollection<QuizSummaryDto> quizzes = [];
    [ObservableProperty] private QuizSummaryDto? selectedQuiz;
    [ObservableProperty] private string joinCode = string.Empty;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string welcomeText = string.Empty;

    public LobbyPageModel(ApiService api, HubService hub, SessionStore session)
    {
        _api = api;
        _hub = hub;
        _session = session;
    }

    public async Task InitializeAsync()
    {
        WelcomeText = $"Welcome, {_session.Username}!";
        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            var quizList = await _api.GetQuizzesAsync();
            Quizzes = new ObservableCollection<QuizSummaryDto>(quizList);

            var roomList = await _api.GetActiveRoomsAsync();
            ActiveRooms = new ObservableCollection<RoomDto>(roomList);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateRoomAsync()
    {
        if (SelectedQuiz is null)
        {
            StatusMessage = "Please select a quiz first.";
            return;
        }
        if (IsBusy) return;
        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            var room = await _api.CreateRoomAsync(new CreateRoomRequest { QuizId = SelectedQuiz.QuizId });
            _session.CurrentRoomCode = room.Code;
            _session.IsRoomHost = true;

            await _hub.ConnectAsync();
            await _hub.JoinRoomAsync(room.Code);

            await Shell.Current.GoToAsync("room");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task JoinRoomAsync()
    {
        if (string.IsNullOrWhiteSpace(JoinCode))
        {
            StatusMessage = "Please enter a room code.";
            return;
        }
        if (IsBusy) return;
        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            await _hub.ConnectAsync();
            await _hub.JoinRoomAsync(JoinCode.Trim().ToUpper());

            var room = await _api.GetRoomAsync(JoinCode.Trim().ToUpper());
            _session.CurrentRoomCode = room.Code;
            _session.IsRoomHost = false;

            await Shell.Current.GoToAsync("room");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task JoinActiveRoomAsync(RoomDto room)
    {
        JoinCode = room.Code;
        await JoinRoomAsync();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        _session.Clear();
        try { await _hub.DisconnectAsync(); } catch { }
        await Shell.Current.GoToAsync("//login");
    }
}
